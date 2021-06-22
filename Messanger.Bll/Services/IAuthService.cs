using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.User;

namespace Messanger.Bll.Services
{
    /// <summary>
    /// Account auth service abstraction.
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// Registers user and assigns unique account id.
        /// </summary>
        /// <param name="registerDto">Contract for registration.</param>
        /// <returns>Returns jwt token or <c>null</c> if login already existed.</returns>
        /// <exception cref="ArgumentNullException">Throws when one of the arguments is null.</exception>
        Task<string> RegisterAsync(UserRegisterDto registerDto);

        /// <summary>
        /// Login user and returns jwt token.
        /// </summary>
        /// <param name="loginDto">Account login.</param>
        /// <returns>Returns user account id or <c>null</c> if user wasn't found or password is invalid.</returns>
        Task<string> LoginAsync(UserLoginDto loginDto);

        string SecurityKey { set; }
    }
}
