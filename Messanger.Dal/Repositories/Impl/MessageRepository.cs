using Messanger.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Dal.Context;

namespace Messanger.Dal.Repositories.Impl
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetLastMessagesAsync()
        {
            return await Set.OrderByDescending(x => x.Timestamp).Take(100).ToListAsync();
        }
    }
}
