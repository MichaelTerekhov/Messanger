using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.Messages;
using Messanger.Dal.Entities;

namespace Messanger.Bll.Mappers
{
    public static class MessageMapper
    {
        public static Message Map(MessageInLineDto obj)
        {
            return obj == null
                ? null
                : new Message
                {
                    Text = obj.Text
                };
        }

        public static MessageInLineDto Map(Message obj)
        {
            return obj == null
                ? null
                : new MessageInLineDto
                {
                    Text = obj.Text,
                    Time = (DateTimeOffset)obj.Timestamp
                };
        }
    }
}
