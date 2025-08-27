import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../models/chat_models.dart';
import '../provider/global.dart';
import '../provider/chat_provider.dart';
import 'chat_room_screen.dart';
import 'nickname_screen.dart';

class ChatLobbyScreen extends ConsumerStatefulWidget {
  const ChatLobbyScreen({super.key});

  @override
  ConsumerState<ChatLobbyScreen> createState() => _ChatLobbyScreenState();
}

class _ChatLobbyScreenState extends ConsumerState<ChatLobbyScreen> {
  List<ChatRoom> _rooms = [];
  bool _isLoading = false;

  @override
  void initState() {
    super.initState();
    _loadChatRooms();
  }

  Future<void> _loadChatRooms() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final repository = ref.read(chatLobbyProvider);
      final rooms = await repository.getChatRooms();
      setState(() {
        _rooms = rooms;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
      });

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('채팅방 목록을 불러오는데 실패했습니다: $e'),
            backgroundColor: Colors.red,
          ),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    // 사용자 닉네임 가져오기
    final nicknameAsync = ref.watch(userNicknameProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('채팅방 목록'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: () {
              // 데이터 새로고침
              _loadChatRooms();
            },
          ),
          PopupMenuButton(
            itemBuilder: (context) => [
              PopupMenuItem(
                value: 'logout',
                child: const Row(
                  children: [
                    Icon(Icons.logout),
                    SizedBox(width: 8),
                    Text('로그아웃'),
                  ],
                ),
              ),
            ],
            onSelected: (value) {
              if (value == 'logout') {
                _logout();
              }
            },
          ),
        ],
      ),
      body: Column(
        children: [
          // 닉네임 표시 - FutureProvider 사용
          nicknameAsync.when(
            data: (nickname) {
              if (nickname != null) {
                return Container(
                  width: double.infinity,
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    color: Colors.deepPurple.withOpacity(0.1),
                  ),
                  child: Text(
                    '환영합니다, $nickname님!',
                    style: const TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                    ),
                    textAlign: TextAlign.center,
                  ),
                );
              }
              return const SizedBox.shrink();
            },
            loading: () => const SizedBox.shrink(),
            error: (error, stack) => const SizedBox.shrink(),
          ),
          Expanded(
            child: _isLoading
                ? const Center(child: CircularProgressIndicator())
                : _rooms.isEmpty
                ? const Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.chat_bubble_outline,
                          size: 64,
                          color: Colors.grey,
                        ),
                        SizedBox(height: 16),
                        Text(
                          '사용 가능한 채팅방이 없습니다.',
                          style: TextStyle(fontSize: 16, color: Colors.grey),
                        ),
                      ],
                    ),
                  )
                : ListView.builder(
                    itemCount: _rooms.length,
                    itemBuilder: (context, index) {
                      final room = _rooms[index];
                      return _buildRoomCard(context, room);
                    },
                  ),
          ),
        ],
      ),
    );
  }

  Widget _buildRoomCard(BuildContext context, ChatRoom room) {
    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      child: ListTile(
        leading: CircleAvatar(
          backgroundColor: Colors.deepPurple,
          child: Text(
            room.name.substring(0, 1),
            style: const TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
        title: Text(
          room.name,
          style: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        subtitle: Text('참여자: ${room.userCount}명'),
        trailing: const Icon(Icons.arrow_forward_ios),
        onTap: () => _joinRoom(context, room),
      ),
    );
  }

  Future<void> _joinRoom(BuildContext context, ChatRoom room) async {
    // 서버 연결 없이 바로 채팅방으로 이동
    Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (context) => ChatRoomScreen(room: room)));
  }

  Future<void> _logout() async {
    // 저장된 닉네임과 서버 URL 삭제
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.remove('nickname');
    await prefs.remove('serverUrl');

    if (mounted) {
      Navigator.of(context).pushAndRemoveUntil(
        MaterialPageRoute(builder: (context) => const NicknameScreen()),
        (route) => false,
      );
    }
  }
}
