using Messanger.Bll.Contracts.Rooms;
using Messanger.Dal.Entities;
using Messanger.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messanger.Bll.Services.Impl
{
    public class RoomService : IRoomService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Room> _roomRepository;

        public RoomService(IAccountRepository accountRepository, IRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<RoomViewDto>> GetAllAvailableRoomsForYouAsync(Guid userId)
        {
            var user = await _accountRepository.GetByIdAsync(userId);

            if (user is null)
                return null;

            var roomHistory = await _roomRepository.GetAllAsync();

            var rooms = new List<RoomViewDto>();
            foreach (var item in roomHistory)
            {
                var splittedIds = item.Name.Split("|");
                if (userId.ToString() != splittedIds[0] &&
                    userId.ToString() != splittedIds[1])
                    continue;

                var roomViewModel = new RoomViewDto
                {
                    RoomSystemName = item.Name
                };


                if (user.Id.ToString() == splittedIds[0])
                {
                    var toChatWithUser = await _accountRepository.GetByIdAsync(Guid.Parse(splittedIds[1]));
                    roomViewModel.EligibleUserToChatWith = toChatWithUser == null
                        ? $"MessangerUser-{Guid.NewGuid()}"
                        : toChatWithUser.Login;
                    roomViewModel.EligibleUserAvatarLink = toChatWithUser?.AvatarLink;
                }
                else
                {
                    var toChatWithUser = await _accountRepository.GetByIdAsync(Guid.Parse(splittedIds[0]));
                    roomViewModel.EligibleUserToChatWith = toChatWithUser == null
                        ? $"MessangerUser-{Guid.NewGuid()}"
                        : toChatWithUser.Login;
                    roomViewModel.EligibleUserAvatarLink = toChatWithUser?.AvatarLink;
                }
                rooms.Add(roomViewModel);
            }
            return rooms.AsEnumerable();
        }
    }
}
