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
        Task<TEntity> InsertAsync(TEntity entity);
        Task DeleteAsync(int id);
        void UpdateAsync(TEntity entityToUpdate);
    }
}
