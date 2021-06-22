using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.User
{
    public class AccountViewDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        [JsonPropertyName("mobile")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("creationTime")]
        public DateTimeOffset CreationTime { get; set; }
    }
}
