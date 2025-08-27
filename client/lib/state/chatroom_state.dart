import '../models/chat_models.dart';

/// 간단한 채팅방 상태
class ChatRoomState {
  final List<ChatMessage> messages;
  final String roomId;

  const ChatRoomState({required this.messages, required this.roomId});

  ChatRoomState copyWith({List<ChatMessage>? messages, String? roomId}) {
    return ChatRoomState(
      messages: messages ?? this.messages,
      roomId: roomId ?? this.roomId,
    );
  }
}
