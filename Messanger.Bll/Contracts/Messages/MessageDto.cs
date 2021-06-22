using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.Messages
{
    public class MessageDto
    {
        [JsonPropertyName("to")]
        public string ToUser { get; set; }

        [JsonPropertyName("message")]
        public string Text { get; set; }
    }
}
