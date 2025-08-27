using ChatServer.Models;
using ChatServer.Models.Dto;
using ChatServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatLobbyController(ChatLobbyService _chatLobbyService,IHubContext<ChatHub> _hubContext) : ControllerBase
{
    [HttpGet("chatrooms")]
    public IEnumerable<ChatRoomRes> GetChatRooms()
    {
        var list =
            _chatLobbyService.GetChatRoomAll();

        return list.Select(x => new ChatRoomRes()
        {
            Uid = x.Uid,
            Name = x.Name,
            UserCount = x.UserCount
        });
    }

    [HttpPost("message")]
    public async Task SendAllMessage([FromBody] SendNoticeReq req)
    {
        if (string.IsNullOrEmpty(req.chatUid) || string.IsNullOrEmpty(req.message))
        {
            throw new ArgumentException("Chat UID and message cannot be null or empty.");
        }

        var chatRoom = _chatLobbyService.GetChatRoom(req.chatUid) ?? throw new Exception($"Chat room with UID {req.chatUid} not found.");
        await _hubContext.Clients.Group(chatRoom.GroupId).SendAsync("ReceiveMessage", new ChatMessage("System","System", req.message, req.chatUid, ChatMessageType.System));
    }
}

