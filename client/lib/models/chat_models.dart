class ChatRoom {
  final String uid;
  final String name;
  final int userCount;

  ChatRoom({required this.uid, required this.name, required this.userCount});

  factory ChatRoom.fromJson(Map<String, dynamic> json) {
    return ChatRoom(
      uid: json['uid'],
      name: json['name'],
      userCount: json['userCount'] ?? 0,
    );
  }
}

enum ChatMessageType { system, user, ai }

class ChatMessage {
  final String userName;
  final String message;
  final String chatUid;
  final String userUid;
  final ChatMessageType type;

  ChatMessage({
    required this.userName,
    required this.message,
    required this.chatUid,
    required this.userUid,
    required this.type,
  });

  factory ChatMessage.fromJson(Map<String, dynamic> json) {
    ChatMessageType messageType;
    switch (json['type']?.toString().toLowerCase()) {
      case 'system':
        messageType = ChatMessageType.system;
        break;
      case 'user':
        messageType = ChatMessageType.user;
        break;
      case 'ai':
        messageType = ChatMessageType.ai;
        break;
      default:
        messageType = ChatMessageType.user;
    }

    return ChatMessage(
      userName: json['userName'] ?? '',
      message: json['message'] ?? '',
      chatUid: json['chatUid'] ?? '',
      type: messageType,
      userUid: json['userUid'] ?? '',
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'userName': userName,
      'message': message,
      'chatUid': chatUid,
      'type': type.index,
      'userUid': userUid,
    };
  }
}

class JoinMessage {
  final String userUid;
  final List<ChatMessage> messages;

  JoinMessage({required this.userUid, required this.messages});

  factory JoinMessage.fromJson(Map<String, dynamic> json) {
    List<ChatMessage> messageList = [];

    if (json['messages'] != null) {
      messageList = (json['messages'] as List)
          .map(
            (messageJson) =>
                ChatMessage.fromJson(messageJson as Map<String, dynamic>),
          )
          .toList();
    }

    return JoinMessage(userUid: json['userUid'] ?? '', messages: messageList);
  }

  Map<String, dynamic> toJson() {
    return {
      'userUid': userUid,
      'messages': messages.map((message) => message.toJson()).toList(),
    };
  }
}
