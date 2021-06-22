using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.Rooms;

namespace Messanger.Bll.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomViewDto>> GetAllAvailableRoomsForYouAsync(Guid userId);
    }
}
