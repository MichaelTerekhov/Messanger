using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.User
{
    public class AccountDto
    {
        /// <summary>
        /// Account id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Account username
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Hashcode of user password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Users mobile phone
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// The link to auto-generated user avatar
        /// </summary>
        public string AvatarLink { get; set; }

        /// <summary>
        /// The type of user account
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Date and Time of account creation
        /// </summary>
        public DateTimeOffset AccountCreationTime { get; set; }

        /// <summary>
        /// Enamle to check you in which room you now
        /// </summary>
        public string CurrentRoom { get; set; }
    }
}
