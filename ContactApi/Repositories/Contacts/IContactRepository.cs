using System.Threading.Tasks;
using DataAccess;
using ContactApi.Models;
using System.Collections.Generic;

namespace ContactApi.Repository
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<bool> MyCustomRepositoryMethodExampleAsync();
        Task<IEnumerable<TEntity>> GetByPageIndex<TEntity>(int page, int pageSize);

    }
}
