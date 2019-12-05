using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IGenericRepository<TEntity>
    {
        IDbConnection GetOpenConnection();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(int id);
        Task<int> InsertAsync(TEntity entity);
        void DeleteAsync(int id);
        void UpdateAsync(TEntity entityToUpdate);
    }
}
