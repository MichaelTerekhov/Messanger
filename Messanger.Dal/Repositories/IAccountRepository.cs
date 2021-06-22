using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Dal.Entities;

namespace Messanger.Dal.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByLoginAsync(string login);
    }
}
