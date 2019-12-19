using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// The concrete implementation of a Document repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DocumentRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private string _connectionString;
        private DbConnectionTypes _dbType;

        public DocumentRepository(string connectionString)
        {
            _dbType = DbConnectionTypes.DOCUMENT;
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            return DbConnectionFactory.GetDbConnection(_dbType, _connectionString);
        }

        public abstract Task DeleteAsync(int id);
        public abstract Task<IEnumerable<TEntity>> GetAllAsync();
        public abstract Task<TEntity> FindAsync(int id);
        public abstract Task<TEntity> InsertAsync(TEntity entity);
        public abstract void UpdateAsync(TEntity entityToUpdate);
    }
}
