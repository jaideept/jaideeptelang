using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace ContactApi
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<bool> MyCustomRepositoryMethodExampleAsync();
    }
}
