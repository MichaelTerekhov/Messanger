using Messanger.Bll.Contracts.Messages;
using Messanger.Bll.Contracts.Rooms;
using Messanger.Bll.Contracts.User;
using Messanger.Bll.Mappers;
using Messanger.Bll.Services;
using Messanger.Dal.Context;
using Messanger.Dal.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messanger.Bll.Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessangerHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public DatabaseContext _dbContext;

        public static List<AccountDto> connections = new();

        private static List<RoomDto> chatRooms = new();

        private static Dictionary<string, string> signalConnections = new();

        public MessangerHub(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<AccountDto> GetUsers(string roomName) =>
            connections.Where(u => u.CurrentRoom == roomName).ToList();

        public IEnumerable<AccountDto> GetAllAvailableUsers() => connections;

        public IEnumerable<RoomViewDto> GetAllPossibleRoomsToChatWith()
        {
            var user = _dbContext.Accounts.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));

            if (user is null)
                return new List<RoomViewDto> { };
            var roomHistory = _dbContext.Rooms.Where(m => m.Name.Contains(user.Id.ToString()))
                .ToList();

            var messagesDto = new List<RoomViewDto>();
            foreach (var item in roomHistory)
            {
                var roomViewModel = new RoomViewDto
                {
                    RoomSystemName = item.Name
                };
                var splittedIds = item.Name.Split("|");

                if (user.Id.ToString() == splittedIds[0])
                {
                    var toChatWithUser = _dbContext.Accounts.FirstOrDefault(u => u.Id.ToString() == splittedIds[1]);
                    roomViewModel.EligibleUserToChatWith = toChatWithUser != null
                        ? toChatWithUser.Login
                        : $"MessangerUser-{Guid.NewGuid()}";
                }
                else
                {
                    var toChatWithUser = _dbContext.Accounts.FirstOrDefault(u => u.Id.ToString() == splittedIds[0]);
                    roomViewModel.EligibleUserToChatWith = toChatWithUser != null
                        ? toChatWithUser.Login
                        : $"MessangerUser-{Guid.NewGuid()}";
                }
                messagesDto.Add(roomViewModel);
            }
            return messagesDto.AsEnumerable();
        }

        public IEnumerable<MessageInLineDto> GetMessageHistory(string roomName)
        {
            var user = _dbContext.Accounts.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));
            var room = _dbContext.Rooms.FirstOrDefault(r => r.Name == roomName);
            var availableUsersInChatRoom = roomName.Split("|");

            if (user?.Id.ToString() != availableUsersInChatRoom[0] &&
                user?.Id.ToString() != availableUsersInChatRoom[1])
                return new List<MessageInLineDto> { };

            var messageHistory = _dbContext.Messages.Where(m => m.Room_Id == room.Id)
                .OrderByDescending(m => m.Timestamp)
                .Take(40)
                .AsEnumerable()
                .Reverse()
                .ToList();

            var messagesDto = new List<MessageInLineDto>();
            foreach (var item in messageHistory)
            {
                var messageOwner = _dbContext.Accounts.FirstOrDefault(u => u.Id == item.Account_Id);
                var messageLineDto = MessageMapper.Map(item);
                messageLineDto.Username = messageOwner.Login;
                messagesDto.Add(messageLineDto);
            }
            return messagesDto.AsEnumerable();
        }

        public void SendToRoom(string roomName, string message)
        {
            try
            {
                var messageDto = JsonSerializer.Deserialize<MessageDto>(message);

                if (messageDto is null)
                    throw new ArgumentNullException();

                var user = _dbContext.Accounts.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));
                var room = _dbContext.Rooms.FirstOrDefault(r => r.Name == roomName);

                var availableUsersInChatRoom = roomName.Split("|");

                var user1 = availableUsersInChatRoom[0];
                var user2 = availableUsersInChatRoom[1];

                if (user?.Id.ToString() != user1 &&
                    user?.Id.ToString() != user2)
                    throw new AccessViolationException();

                Message msg = new Message()
                {
                    Id = Guid.NewGuid(),
                    Text = messageDto.Text,
                    Account_Id = user.Id,
                    Room_Id = room.Id,
                    Timestamp = DateTime.UtcNow,
                };
                _dbContext.Messages.Add(msg);
                _dbContext.SaveChanges();

                var messageInLineDto = MessageMapper.Map(msg);
                messageInLineDto.Username = user.Login;
                Clients.Group(roomName).SendAsync("newMessage", messageInLineDto);

            }
            catch (Exception)
            {
                Clients.Caller.SendAsync("onError", "Message not send!");
            }
        }

        public void Join(string roomName)
        {
            try
            {
                var user = connections.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));
                var availableUsersInChatRoom = roomName.Split("|");

                if (user?.Id.ToString() != availableUsersInChatRoom[0] &&
                    user?.Id.ToString() != availableUsersInChatRoom[1])
                    throw new AccessViolationException();

                if (user is not null && user.CurrentRoom != roomName)
                {
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);
                    Leave(user.CurrentRoom ?? string.Empty);

                    Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        private void Leave(string roomName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public RoomViewDto CreateRoom(string chatWithUserId)
        {
            try
            {
                var actualUser = _dbContext.Accounts.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));
                var userChatWith = _dbContext.Accounts.FirstOrDefault(u => u.Id.ToString() == chatWithUserId);

                if (userChatWith is null)
                    Clients.Caller.SendAsync("onError", "this user don`t exists!");

                var roomName = $"{actualUser?.Id}|{userChatWith?.Id}";

                if (_dbContext.Rooms.Any(r => r.Name == roomName))
                {
                    Clients.Caller.SendAsync("onError", "Another chat room with this name exists");
                    return null;
                }
                else
                {
                    var room = new Room()
                    {
                        Id = Guid.NewGuid(),
                        Name = roomName
                    };

                    _dbContext.Rooms.Add(room);
                    _dbContext.SaveChanges();

                    if (room is not null)
                    {
                        chatRooms.Add(RoomMapper.Map(room));
                    }

                    var roomToReturn = RoomMapper.MapView(room);
                    roomToReturn.EligibleUserAvatarLink = userChatWith.AvatarLink;
                    roomToReturn.EligibleUserToChatWith = userChatWith.Login;

                    return roomToReturn;
                }

            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "Couldn't create chat room: " + ex.Message);
                return null;
            }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _dbContext.Accounts.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));

                var internalAccount = AccountMapper.Map(user);

                if (connections.All(u => u.Login != internalAccount?.Login))
                {
                    connections.Add(internalAccount);
                    signalConnections.Add(internalAccount.Id.ToString(), Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getProfileInfo", user?.Login);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = connections.FirstOrDefault(u => u.Id == ClaimsIdentityService.GetIdFromToken(Context.User));
                connections.Remove(user);

                Clients.OthersInGroup(user.CurrentRoom).SendAsync("offline", user);

                signalConnections.Remove(user.Id.ToString());
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
