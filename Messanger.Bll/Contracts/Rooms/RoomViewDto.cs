using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.Rooms
{
    public class RoomViewDto
    {
        [JsonPropertyName("systemName")]
        public string RoomSystemName { get; set; }

        [JsonPropertyName("avatar")]
        public string EligibleUserAvatarLink { get; set; }

        [JsonPropertyName("login")]
        public string EligibleUserToChatWith { get; set; }
    }
}
