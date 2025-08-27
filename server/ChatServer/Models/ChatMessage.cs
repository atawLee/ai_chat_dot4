

namespace ChatServer.Models;

public record ChatMessage(string UserName, string UserUid, string Message, string ChatUid, ChatMessageType Type);
