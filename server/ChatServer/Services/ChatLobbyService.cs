using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Services;


/// <summary>
/// 채팅방 입장과 퇴장, 생성, 삭제 등을 관리하는 서비스
/// </summary>
public class ChatLobbyService
{
    private readonly Dictionary<string, ChatRoom> _chatRooms = new()
    {
        {
            "general-room-001",
            new ChatRoom
            {
                Uid = "general-room-001",
                Name = "일반 채팅방",
                Password = null,
                GroupId = "general-group-001",
                
            }
        },
        {
            "welcome-room-002", 
            new ChatRoom
            {
                Uid = "welcome-room-002",
                Name = "환영 채팅방",
                Password = null,
                GroupId = "welcome-group-002",
            }
        }
    };

    public ChatRoom GetChatRoom(string chatUid)
    {
        var chatroom = _chatRooms.GetValueOrDefault(chatUid);
        return chatroom ?? throw new Exception($"Not Found ChatRoom");
    }

    public ChatRoom CreateChatRoomAndReturn(string name)
    {
        string chatRoomUid = Guid.NewGuid().ToString();
        string privateGroupId = Guid.NewGuid().ToString();

        var chatRoom = new ChatRoom
        {
            Uid = chatRoomUid,
            Name = name,
            Password = null,
            GroupId = privateGroupId
        };

        _chatRooms[chatRoomUid] = chatRoom;

        return chatRoom;
    }

    public void JoinChatRoom(string chatUid, string userConnectionId, string username)
    {
        var room =  _chatRooms.GetValueOrDefault(chatUid) ?? throw new Exception($"Not Found ChatRoom");
        var chatUser = new ChatUser()
        {
            UserName = username,
            UserUid = userConnectionId
        };

        room.AddUser(chatUser);
    }

    public List<ChatRoom> GetChatRoomAll()
    {
        return this._chatRooms.Values.ToList();
    }

    public void RemoveUserFromAllRooms(string connectionId)
    {
        foreach (var chatRoom in _chatRooms.Values)
        {
            // ConcurrentBag에서 특정 사용자 제거는 직접적으로 불가능하므로
            // 새로운 ConcurrentBag을 만들어서 해당 사용자를 제외하고 다시 채움
            var existingUsers = chatRoom.Users.Where(u => u.UserUid != connectionId).ToArray();

            // Users를 새로 초기화하고 기존 사용자들을 다시 추가
            while (chatRoom.Users.TryTake(out _)) { } // 모든 사용자 제거

            foreach (var user in existingUsers)
            {
                chatRoom.Users.Add(user);
            }
        }
    }
}