using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Dal.Entities
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [MaxLength(2)]
        public ICollection<Account> Users { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
