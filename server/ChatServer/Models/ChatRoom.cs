using System.Collections.Concurrent;


namespace ChatServer.Models;

public class ChatRoom
{
    /// <summary>
    /// user에게 공개되는 고유 아이디 
    /// </summary>
    public string Uid { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string? Password { get; set; }
    /// <summary>
    /// signalR group name 
    /// </summary>
    public string GroupId { get; set; } = string.Empty;

    public int UserCount => Users.Count;

    public ConcurrentQueue<ChatMessage> Messages { get; } = [];

    public ConcurrentBag<ChatUser> Users { get; } = [];

    public void AddMessage(ChatMessage message)
    {
        Messages.Enqueue(message);
            
        // 메시지가 20개를 초과하면 오래된 메시지 제거
        while (Messages.Count > 20)
        {
            Messages.TryDequeue(out _);
        }
    }

    public void AddUser(ChatUser user)
    {
        Users.Add(user);
    }
}

public class ChatUser
{
    public string UserUid { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}