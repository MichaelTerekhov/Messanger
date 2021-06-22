using Messanger.Bll.Contracts.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messanger.Bll.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountViewDto>> GetAllAccountsAsync();

        Task<AccountDto> GetByLoginAsync(string login);

        Task<bool> AddAsync(AccountDto registeredAcc);
    }
}
