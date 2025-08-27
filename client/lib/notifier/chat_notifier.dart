import 'dart:async';

import 'package:ai_chat/models/chat_models.dart';
import 'package:ai_chat/provider/chat_provider.dart';
import 'package:ai_chat/repository/chat_client.dart';
import 'package:ai_chat/state/chatroom_state.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

class ChatRoomStateNotifier extends AsyncNotifier<ChatRoomState> {
  late ChatClient chatClient;
  late String roomId;
  ChatRoomStateNotifier();

  @override
  FutureOr<ChatRoomState> build() {
    chatClient = ref.read(chatClientProvider);
    chatClient.onMessageReceived = _onMessageReceived;

    return ChatRoomState(messages: const [], roomId: roomId);
  }

  void _onMessageReceived(ChatMessage message) {
    state.when(
      data: (currentState) {
        // 데이터 상태일 때: 기존 메시지에 새 메시지 추가
        final newMessages = [...currentState.messages, message];
        final newState = currentState.copyWith(messages: newMessages);
        state = AsyncValue.data(newState);
      },
      loading: () {
        // 로딩 상태일 때: 새 메시지만 포함한 상태로 변경
        final newState = ChatRoomState(messages: [message], roomId: roomId);
        state = AsyncValue.data(newState);
      },
      error: (error, stackTrace) {
        // 에러 상태일 때: 에러를 무시하고 새 메시지로 복구
        final newState = ChatRoomState(messages: [message], roomId: roomId);
        state = AsyncValue.data(newState);
      },
    );
  }

  /// 메시지 추가
  Future<void> addMessage(ChatMessage message) async {
    state = const AsyncValue.loading();

    try {
      final currentState = await future;
      final newMessages = [...currentState.messages, message];
      final newState = currentState.copyWith(messages: newMessages);

      state = AsyncValue.data(newState);
    } catch (error, stackTrace) {
      state = AsyncValue.error(error, stackTrace);
    }
  }

  /// 채팅방 연결 및 초기화
  Future<void> initializeChatRoom() async {
    state = const AsyncValue.loading();

    try {
      // 여기서 ChatClient 연결 로직 처리 (arg로 받은 roomId 사용 가능)
      // family parameter 접근

      await Future.delayed(const Duration(seconds: 1)); // 시뮬레이션

      final initialState = const ChatRoomState(messages: [], roomId: '');
      state = AsyncValue.data(initialState);
    } catch (error, stackTrace) {
      state = AsyncValue.error(error, stackTrace);
    }
  }

  /// 메시지 전송
  Future<void> sendMessage(String userName, String messageText) async {
    try {
      // 실제 서버로 메시지 전송 로직

      // 임시로 로컬에 메시지 추가
      final message = ChatMessage(
        userName: userName,
        message: messageText,
        chatUid: roomId,
        type: ChatMessageType.user,
        userUid: '',
      );

      await chatClient.sendMessage(message);

      await addMessage(message);
    } catch (error, stackTrace) {
      state = AsyncValue.error(error, stackTrace);
    }
  }

  /// 채팅방 연결 해제
  Future<void> disconnect() async {
    state = const AsyncValue.loading();

    try {
      // ChatClient disconnect 로직
      await Future.delayed(const Duration(milliseconds: 500));

      state = const AsyncValue.data(ChatRoomState(messages: [], roomId: ''));
    } catch (error, stackTrace) {
      state = AsyncValue.error(error, stackTrace);
    }
  }
}
