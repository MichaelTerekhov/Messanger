using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.Permissions;
using Messanger.Bll.Contracts.User;
using Messanger.Bll.Generators;

namespace Messanger.Bll.Services.Impl
{
    public class AuthService : IAuthService
    {

        public string SecurityKey { get; set; }

        private readonly IAccountService _accountService;

        private readonly IHashGenerator _hashGenerator;

        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(IAccountService accountService,
            IHashGenerator hashGenerator,
            IJwtGenerator jwtGenerator)
        {
            _accountService = accountService;
            _hashGenerator = hashGenerator;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<string> LoginAsync(UserLoginDto accountLoginDto)
        {
            if (accountLoginDto == null)
                throw new ArgumentNullException(nameof(accountLoginDto));

            if (string.IsNullOrEmpty(accountLoginDto.Login) || string.IsNullOrEmpty(accountLoginDto.Password))
                return null;

            var accFromBase = await _accountService.GetByLoginAsync(accountLoginDto.Login);
            if (accFromBase == null)
                return null;

            if (!CheckPasswords(accFromBase.PasswordHash, accountLoginDto.Password))
                return null;

            return _jwtGenerator.GenerateJwt(accFromBase, SecurityKey);
        }

        public async Task<string> RegisterAsync(UserRegisterDto accountRegisterDto)
        {
            if (accountRegisterDto == null)
                throw new ArgumentNullException(nameof(accountRegisterDto));

            if (string.IsNullOrEmpty(accountRegisterDto.Password))
                return null;

            if (await _accountService.GetByLoginAsync(accountRegisterDto.Login) != null)
                return null;

            var newId = Guid.NewGuid();
            var accountForRegister = new AccountDto
            {
                Id = newId,
                AccountType = Roles.User,
                MobilePhone = accountRegisterDto.MobilePhone,
                PasswordHash = _hashGenerator.GenerateHash(accountRegisterDto.Password),
                Login = accountRegisterDto.Login,
                AccountCreationTime = DateTimeOffset.UtcNow,
                AvatarLink = $"https://i.pravatar.cc/150?u={newId}"
            };

            if (await _accountService.AddAsync(accountForRegister))
                return _jwtGenerator.GenerateJwt(accountForRegister, SecurityKey);

            return null;
        }

        private bool CheckPasswords(string passwordHash, string passwordFromLogin)
        {
            return _hashGenerator.GenerateHash(passwordFromLogin) == passwordHash;
        }
    }
}
