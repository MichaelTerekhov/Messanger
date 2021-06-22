using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messanger.Dal.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        public string Login { get; set; }

        [MinLength(6, ErrorMessage = "Password must be minimum 6 symbols length.")]
        public string PasswordHash { get; set; }

        public string AvatarLink { get; set; }

        public string AccountType { get; set; }

        public string PhoneNumber { get; set; }

        public DateTimeOffset AccountCreationTime { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}