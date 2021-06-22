using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Dal.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
        
        public Guid Account_Id { get; set; }
        
        [ForeignKey("Account_Id")]
        public Account User { get; set; }

        public Guid Room_Id { get; set; }

        [ForeignKey("Room_Id")]
        public Room Room { get; set; }

    }
}
