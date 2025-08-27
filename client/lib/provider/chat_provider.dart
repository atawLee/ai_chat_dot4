import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../repository/chat_lobby_repository.dart';
import '../repository/chat_client.dart';
import '../notifier/chat_notifier.dart';
import '../state/chatroom_state.dart';
import 'global.dart';

// ChatLobbyRepository를 제공하는 단순 Provider
final chatLobbyProvider = Provider<ChatLobbyRepository>((ref) {
  final apiSetting = ref.watch(apiProvider);
  return ChatLobbyRepository(baseUrl: apiSetting.defaultAddress);
});

// ChatClient를 싱글턴으로 제공하는 Provider
final chatClientProvider = Provider<ChatClient>((ref) {
  final apiSetting = ref.watch(apiProvider);
  return ChatClient(baseUrl: apiSetting.defaultAddress);
});

final chatRoomProvider =
    AsyncNotifierProvider<ChatRoomStateNotifier, ChatRoomState>(() {
      // roomId는 ChatRoomStateNotifier를 사용할 때 설정해야 합니다
      return ChatRoomStateNotifier(); // 기본값, 실제로는 초기화 시 설정
    });
