using AI.Core;
using ChatServer.Models;
using ChatServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer;

public class ChatHub(ChatLobbyService _lobbyService, AiService _aiService) : Hub
{

    public async Task JoinGroup(string chatUid, string userName)
    {
        ChatRoom chatRoom = _lobbyService.GetChatRoom(chatUid) ?? throw new Exception($"Chat room with UID {chatUid} not found.");
        _lobbyService.JoinChatRoom(chatUid,Context.ConnectionId,userName);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom.GroupId);
        await Clients.Caller.SendAsync("JoinMessage",
            new JoinMessage()
            {
                UserUid = Context.ConnectionId,
                Messages = chatRoom.Messages.ToList()
            });
    }
    
    public async Task SendMessageToGroup(ChatMessage message)
    {
        _ = message?.ChatUid ?? throw new Exception("message is null");
        var chatRoom = _lobbyService.GetChatRoom(message.ChatUid);

        chatRoom.AddMessage(message);
        await Clients.Group(chatRoom.GroupId).SendAsync("ReceiveMessage", message);

        if (message.Type == ChatMessageType.AI)
        {
            var aiAnswer = await _aiService.InvokePromptAsync(message.Message);
            ChatMessage aiMessage = new("AI", "AI", aiAnswer, message.ChatUid, ChatMessageType.AI);
            await Clients.Group(chatRoom.GroupId).SendAsync("ReceiveMessage", aiMessage);
        }
    }
    public async Task CreateChatRoom(string? chatRoomName)
    {
        if (string.IsNullOrEmpty(chatRoomName))
        {
            throw new ArgumentException("Chat UID cannot be null or empty.", nameof(chatRoomName));
        }

        var chatRoom = _lobbyService.CreateChatRoomAndReturn(chatRoomName);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom.GroupId);
        await Clients.Group(chatRoom.GroupId).SendAsync("ReceiveMessage", "System",
            $"{Context.ConnectionId} has created and joined the group {chatRoom.Name}.");
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            Console.WriteLine($"연결 해제 이유: {exception.Message}");
        }

        // 모든 채팅방에서 해당 사용자 제거
        _lobbyService.RemoveUserFromAllRooms(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}