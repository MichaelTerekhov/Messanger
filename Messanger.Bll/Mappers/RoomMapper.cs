using Messanger.Bll.Contracts.Rooms;
using Messanger.Bll.Contracts.User;
using Messanger.Dal.Entities;

namespace Messanger.Bll.Mappers
{
    public static class RoomMapper
    {
        public static Room Map(RoomDto obj)
        {
            return obj == null
                ? null
                : new Room
                {
                    Id = obj.Id,
                    Name = obj.Name
                };
        }

        public static RoomDto Map(Room obj)
        {
            return obj == null
                ? null
                : new RoomDto
                {
                    Id = obj.Id,
                    Name = obj.Name
                };
        }

        public static RoomViewDto MapView(Room obj)
        {
            return obj == null
                ? null
                : new RoomViewDto
                {
                    RoomSystemName = obj.Name
                };
        }
    }
}
