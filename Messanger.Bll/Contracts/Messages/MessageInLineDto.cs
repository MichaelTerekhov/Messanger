using System;
using System.Text.Json.Serialization;

namespace Messanger.Bll.Contracts.Messages
{
    public class MessageInLineDto
    {
        [JsonPropertyName("login")]
        public string Username { get;set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("messageTime")]
        public DateTimeOffset Time { get; set; }
    }
}
