namespace ChatServer.Models.Dto;

public class ChatRoomRes
{
    public string Uid { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public int UserCount { get; set; }
    
}