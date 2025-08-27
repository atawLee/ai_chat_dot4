import 'package:ai_chat/models/chat_models.dart';
import 'package:signalr_netcore/signalr_client.dart';

class ChatClient {
  HubConnection? _connection;
  final String _baseUrl;

  // 메시지 수신 콜백
  Function(ChatMessage)? onMessageReceived;
  Function(JoinMessage)? onJoin;

  ChatClient({required String baseUrl}) : _baseUrl = baseUrl;

  /// SignalR 연결 초기화
  Future<void> initialize() async {
    _connection = HubConnectionBuilder().withUrl('$_baseUrl/chathub').build();

    // 연결 전에 메시지 핸들러 등록
    _setupMessageHandlers();
  }

  /// 메시지 핸들러 설정
  void _setupMessageHandlers() {
    // ReceiveMessage 핸들러 등록
    _connection!.on('ReceiveMessage', (List<Object?>? arguments) {
      if (arguments != null && arguments.isNotEmpty) {
        var chatMessage = ChatMessage.fromJson(
          arguments[0] as Map<String, dynamic>,
        );

        onMessageReceived?.call(chatMessage);
      }
    });

    _connection!.on('JoinMessage', (List<Object?>? arguments) {
      if (arguments != null && arguments.isNotEmpty) {
        var joinMessage = JoinMessage.fromJson(
          arguments[0] as Map<String, dynamic>,
        );

        onJoin?.call(joinMessage);
      }
    });

    _connection!.on('AiMessage', (List<Object?>? arguments) {
      if (arguments != null && arguments.isNotEmpty) {
        var aiMessage = ChatMessage.fromJson(
          arguments[0] as Map<String, dynamic>,
        );

        onMessageReceived?.call(aiMessage);
      }
    });
  }

  /// 서버에 연결
  Future<bool> connect() async {
    if (_connection == null) {
      await initialize();
    }

    try {
      await _connection!.start();
      print('SignalR 연결 성공');
      return _connection!.state == HubConnectionState.Connected;
    } catch (e) {
      print('연결 실패: $e');
      return false;
    }
  }

  Future<void> sendAiMessage(ChatMessage message) async {
    if (_connection?.state != HubConnectionState.Connected) {
      print('SignalR 연결이 되지 않았습니다.');
      return;
    }

    try {
      await _connection!.invoke('SendMessageToGroup', args: [message.toJson()]);
    } catch (e) {
      print('AI 메시지 전송 실패: $e');
    }
  }

  Future<bool> sendMessage(ChatMessage message) async {
    if (_connection?.state != HubConnectionState.Connected) {
      print('SignalR 연결이 되지 않았습니다.');
      return false;
    }

    try {
      await _connection!.invoke('SendMessageToGroup', args: [message.toJson()]);

      return true;
    } catch (e) {
      print('메시지 전송 실패: $e');
      return false;
    }
  }

  /// 서버 연결 해제
  Future<void> disconnect() async {
    if (_connection != null) {
      await _connection!.stop();
      print('SignalR 연결 해제');
    }
  }

  /// 연결 상태 확인
  bool get isConnected => _connection?.state == HubConnectionState.Connected;

  /// 현재 연결 상태 반환
  HubConnectionState? get currentState => _connection?.state;

  /// 채팅방 그룹 입장
  Future<bool> joinGroup(String chatUid, userName) async {
    if (_connection?.state != HubConnectionState.Connected) {
      print('SignalR 연결이 되지 않았습니다.');
      return false;
    }

    try {
      await _connection!.invoke('JoinGroup', args: [chatUid, userName]);
      print('채팅방 그룹 입장 성공: $chatUid');
      return true;
    } catch (e) {
      print('채팅방 그룹 입장 실패: $e');
      return false;
    }
  }

  /// 리소스 정리
  void dispose() {
    disconnect();
  }
}
