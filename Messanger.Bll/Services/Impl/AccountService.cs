using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.User;
using Messanger.Bll.Mappers;
using Messanger.Dal.Repositories;

namespace Messanger.Bll.Services.Impl
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountViewDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
                return null;

            return accounts.Select(AccountMapper.MapForView).ToList();
        }

        public async Task<AccountDto> GetByLoginAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                return null;

            var account = await _accountRepository.GetAccountByLoginAsync(login);

            return AccountMapper.Map(account);
        }



        public async Task<bool> AddAsync(AccountDto registeredAccount)
        {
            if (registeredAccount == null)
                throw new ArgumentNullException(nameof(registeredAccount));

            var account = AccountMapper.Map(registeredAccount);

            await _accountRepository.AddAsync(account);

            return true;
        }
    }
}
