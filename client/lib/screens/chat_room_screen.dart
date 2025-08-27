import 'package:ai_chat/provider/global.dart';
import 'package:ai_chat/repository/chat_client.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../models/chat_models.dart';

class ChatRoomScreen extends ConsumerStatefulWidget {
  final ChatRoom room;

  const ChatRoomScreen({super.key, required this.room});

  @override
  ConsumerState<ChatRoomScreen> createState() => _ChatRoomScreenState();
}

class _ChatRoomScreenState extends ConsumerState<ChatRoomScreen> {
  final TextEditingController _messageController = TextEditingController();
  final ScrollController _scrollController = ScrollController();
  final List<ChatMessage> _messages = [];

  ChatClient? _chatClient;
  bool _isConnecting = false;
  bool _isConnected = false;
  bool _isAIRequest = false;
  String _nickname = '';

  @override
  void initState() {
    super.initState();
    _initializeChat();
  }

  Future<void> _initializeChat() async {
    setState(() {
      _isConnecting = true;
    });

    try {
      // ApiProvider에서 서버 주소 가져오기
      final apiSetting = ref.read(apiProvider);

      // SharedPreferences에서 닉네임 가져오기
      final prefs = await SharedPreferences.getInstance();
      _nickname = prefs.getString('nickname') ?? '익명';

      // ChatClient 생성 및 초기화
      _chatClient = ChatClient(baseUrl: apiSetting.defaultAddress);

      // 메시지 수신 콜백 등록
      _chatClient!.onMessageReceived = _onMessageReceived;
      _chatClient!.onJoin = _onJoin;

      await _chatClient!.initialize();

      // 서버에 연결
      bool connected = await _chatClient!.connect();

      if (connected) {
        // 채팅방 그룹에 조인
        bool joined = await _chatClient!.joinGroup(widget.room.uid, _nickname);

        setState(() {
          _isConnected = joined;
          _isConnecting = false;
        });

        if (joined) {
          print('채팅방 "${widget.room.name}"에 성공적으로 조인했습니다.');
        } else {
          _showError('채팅방 조인에 실패했습니다.');
        }
      } else {
        setState(() {
          _isConnected = false;
          _isConnecting = false;
        });
        _showError('서버 연결에 실패했습니다.');
      }
    } catch (e) {
      setState(() {
        _isConnecting = false;
        _isConnected = false;
      });
      _showError('채팅 초기화 중 오류가 발생했습니다: $e');
    }
  }

  void _showError(String message) {
    if (mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(message), backgroundColor: Colors.red),
      );
    }
  }

  String _userUid = '';

  void _onJoin(JoinMessage joinMessage) {
    _userUid = joinMessage.userUid;
    _messages.addAll(joinMessage.messages);
    setState(() {});
  }

  /// 메시지 수신 콜백
  void _onMessageReceived(ChatMessage message) {
    if (mounted) {
      setState(() {
        _messages.add(message);
      });

      // 새 메시지가 도착했을 때 자동으로 스크롤을 맨 아래로 이동
      WidgetsBinding.instance.addPostFrameCallback((_) {
        if (_scrollController.hasClients) {
          _scrollController.animateTo(
            _scrollController.position.maxScrollExtent,
            duration: const Duration(milliseconds: 300),
            curve: Curves.easeOut,
          );
        }
      });

      // 선택적: 알림음이나 진동 추가 가능
      print('새 메시지 수신: ${message.userName} - ${message.message}');
    }
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Scaffold(
        appBar: AppBar(
          title: Text(widget.room.name),
          backgroundColor: Theme.of(context).colorScheme.inversePrimary,
          leading: IconButton(
            icon: const Icon(Icons.arrow_back),
            onPressed: () {
              Navigator.of(context).pop();
            },
          ),
        ),
        body: Column(
          children: [
            // 채팅 메시지 목록
            Expanded(
              child: _messages.isEmpty
                  ? const Center(
                      child: Text(
                        '아직 메시지가 없습니다.\n첫 번째 메시지를 보내보세요!',
                        textAlign: TextAlign.center,
                        style: TextStyle(fontSize: 16, color: Colors.grey),
                      ),
                    )
                  : ListView.builder(
                      controller: _scrollController,
                      padding: const EdgeInsets.all(8),
                      itemCount: _messages.length,
                      itemBuilder: (context, index) {
                        final message = _messages[index];
                        final isMyMessage =
                            message.userUid == _userUid; // 임시로 '나'로 설정
                        print(_userUid);
                        return _buildMessageBubble(message, isMyMessage);
                      },
                    ),
            ),
            // 메시지 입력 영역
            Container(
              padding: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: Colors.grey[100],
                boxShadow: [
                  BoxShadow(
                    color: Colors.grey.withOpacity(0.3),
                    spreadRadius: 1,
                    blurRadius: 5,
                    offset: const Offset(0, -2),
                  ),
                ],
              ),
              child: Column(
                children: [
                  // AI 요청 체크박스
                  GestureDetector(
                    onTap: () {
                      setState(() {
                        _isAIRequest = !_isAIRequest;
                      });
                    },
                    child: Row(
                      children: [
                        Checkbox(
                          value: _isAIRequest,
                          onChanged: (bool? value) {
                            setState(() {
                              _isAIRequest = value ?? false;
                            });
                          },
                          activeColor: Colors.deepPurple,
                        ),
                        const Text(
                          'AI에게 요청하기',
                          style: TextStyle(
                            fontSize: 14,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                        const SizedBox(width: 8),
                        Icon(
                          Icons.smart_toy,
                          color: _isAIRequest ? Colors.deepPurple : Colors.grey,
                          size: 20,
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 8),

                  // 메시지 입력 필드
                  Row(
                    children: [
                      Expanded(
                        child: TextField(
                          controller: _messageController,
                          decoration: InputDecoration(
                            hintText: _isAIRequest
                                ? 'AI에게 질문하거나 요청하세요...'
                                : '메시지를 입력하세요...',
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(25),
                              borderSide: BorderSide.none,
                            ),
                            filled: true,
                            fillColor: Colors.white,
                            contentPadding: const EdgeInsets.symmetric(
                              horizontal: 20,
                              vertical: 10,
                            ),
                          ),
                          onSubmitted: (_) =>
                              _isAIRequest ? _sendAiMessage() : _sendMessage(),
                          textInputAction: TextInputAction.send,
                        ),
                      ),
                      const SizedBox(width: 8),
                      FloatingActionButton(
                        onPressed: _isAIRequest ? _sendAiMessage : _sendMessage,
                        backgroundColor: _isAIRequest
                            ? Colors.blue
                            : Colors.deepPurple,
                        mini: true,
                        child: Icon(
                          _isAIRequest ? Icons.smart_toy : Icons.send,
                          color: Colors.white,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildMessageBubble(ChatMessage message, bool isMyMessage) {
    // 메시지 타입에 따른 색상 설정
    Color backgroundColor;
    Color textColor;

    if (isMyMessage) {
      backgroundColor = Colors.deepPurple;
      textColor = Colors.white;
    } else {
      switch (message.type) {
        case ChatMessageType.system:
          backgroundColor = Colors.orange[100]!;
          textColor = Colors.orange[800]!;
          break;
        case ChatMessageType.ai:
          backgroundColor = Colors.blue[100]!;
          textColor = Colors.blue[800]!;
          break;
        case ChatMessageType.user:
        default:
          backgroundColor = Colors.grey[300]!;
          textColor = Colors.black87;
          break;
      }
    }

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        mainAxisAlignment: isMyMessage
            ? MainAxisAlignment.end
            : MainAxisAlignment.start,
        children: [
          if (!isMyMessage) ...[
            CircleAvatar(
              radius: 16,
              backgroundColor: Colors.grey[300],
              child: Text(
                message.userName.substring(0, 1).toUpperCase(),
                style: const TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            const SizedBox(width: 8),
          ],
          Flexible(
            child: Container(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
              decoration: BoxDecoration(
                color: backgroundColor,
                borderRadius: BorderRadius.circular(18),
              ),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  if (!isMyMessage)
                    Text(
                      message.userName,
                      style: TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.bold,
                        color: Colors.grey[600],
                      ),
                    ),
                  Text(
                    message.message,
                    style: TextStyle(color: textColor, fontSize: 16),
                  ),
                  const SizedBox(height: 4),
                ],
              ),
            ),
          ),
          if (isMyMessage) ...[
            const SizedBox(width: 8),
            CircleAvatar(
              radius: 16,
              backgroundColor: Colors.deepPurple[100],
              child: Text(
                message.userName.substring(0, 1).toUpperCase(),
                style: TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.bold,
                  color: Colors.deepPurple,
                ),
              ),
            ),
          ],
        ],
      ),
    );
  }

  String _formatTime(DateTime dateTime) {
    return '${dateTime.hour.toString().padLeft(2, '0')}:${dateTime.minute.toString().padLeft(2, '0')}';
  }

  Future<void> _sendMessage() async {
    final messageText = _messageController.text.trim();
    if (messageText.isEmpty || _chatClient == null) return;

    try {
      // ChatMessage 객체 생성
      final message = ChatMessage(
        userName: _nickname,
        message: messageText,
        chatUid: widget.room.uid,
        type: _isAIRequest ? ChatMessageType.ai : ChatMessageType.user,
        userUid: _userUid,
      );

      // 서버로 메시지 전송
      final success = await _chatClient!.sendMessage(message);

      if (success) {
        print('메시지 전송 성공: $messageText');
        _messageController.clear();

        // 스크롤을 맨 아래로 이동
        WidgetsBinding.instance.addPostFrameCallback((_) {
          if (_scrollController.hasClients) {
            _scrollController.animateTo(
              _scrollController.position.maxScrollExtent,
              duration: const Duration(milliseconds: 300),
              curve: Curves.easeOut,
            );
          }
        });
      } else {
        _showError('메시지 전송에 실패했습니다.');
      }
    } catch (e) {
      _showError('메시지 전송 중 오류가 발생했습니다: $e');
    }
  }

  Future<void> _sendAiMessage() async {
    final messageText = _messageController.text.trim();
    if (messageText.isEmpty || _chatClient == null) return;

    try {
      // ChatMessage 객체 생성
      final message = ChatMessage(
        userName: _nickname,
        message: messageText,
        chatUid: widget.room.uid,
        type: _isAIRequest ? ChatMessageType.ai : ChatMessageType.user,
        userUid: _userUid,
      );

      // 서버로 메시지 전송
      _messageController.clear();
      setState(() {});
      await _chatClient!.sendAiMessage(message);
    } catch (e) {
      _showError('메시지 전송 중 오류가 발생했습니다: $e');
    }
  }

  @override
  void dispose() {
    _messageController.dispose();
    _scrollController.dispose();
    _chatClient?.dispose();
    super.dispose();
  }
}
