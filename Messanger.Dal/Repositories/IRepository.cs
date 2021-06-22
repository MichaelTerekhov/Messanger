using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Dal.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T obj);

        Task RemoveByIdAsync(Guid id);

        Task RemoveAsync(T obj);

        Task UpdateAsync(T obj);
    }
}
