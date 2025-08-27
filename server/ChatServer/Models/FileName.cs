using ChatServer.Models;
namespace ChatServer.Models;


public class JoinMessage
{
    public string UserUid { get; set; } = string.Empty;

    public List<ChatMessage> Messages { get; set; } = [];

}
