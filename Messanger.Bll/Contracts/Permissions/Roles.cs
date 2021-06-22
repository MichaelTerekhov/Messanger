using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.Permissions
{
    public class Roles
    {
        public const string Admin = "ADMIN";

        public const string User = "USER";

        public const string AdminOrUser = Admin + "," + User;
    }
}
