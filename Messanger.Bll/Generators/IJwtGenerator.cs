using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.User;

namespace Messanger.Bll.Generators
{
    /// <summary>
    /// Jwt generation abstraction
    /// </summary>
    public interface IJwtGenerator
    {
        /// <summary>
        /// Creates new Jwt token
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="securityKey">Security key for token</param>
        /// <returns>Jwt token with encrypted [snth to add]</returns>
        string GenerateJwt(AccountDto account, string securityKey);
    }
}
