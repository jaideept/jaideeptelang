using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataAccess;

namespace ContactApi
{
    public class ContactRepository : SqlRepository<Contact>, IContactRepository
    {
        public ContactRepository(string connectionString) : base(connectionString) { }

        public override async void DeleteAsync(int id)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "DELETE FROM Contact WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, System.Data.DbType.Int32);
                await conn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public override async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM Contact";
                return await conn.QueryAsync<Contact>(sql);
            }
        }

        public override async Task<Contact> FindAsync(int id)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM Contact WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, System.Data.DbType.Int32);
                return await conn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public override async Task<int> InsertAsync(Contact entity)
        {
            var inserted = 0;

            using (var conn = GetOpenConnection())
            {
                var sql = "INSERT INTO Contact(FirstName, LastName, Email, Phone, Status) "
                    + "VALUES (@FirstName, @LastName, @Email, @Phone, @Status)";

                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", entity.FirstName, System.Data.DbType.String);
                parameters.Add("@LastName", entity.LastName, System.Data.DbType.String);
                parameters.Add("@Email", entity.Email, System.Data.DbType.String);
                parameters.Add("@Phone", entity.Phone, System.Data.DbType.String);
                parameters.Add("@Status", entity.Status, System.Data.DbType.Boolean);

                inserted += await conn.ExecuteAsync(sql, parameters);
            }

            return inserted;
        }

        public override async void UpdateAsync(Contact entityToUpdate)
        {
            using (var conn = GetOpenConnection())
            {
                var existingEntity = await FindAsync(entityToUpdate.Id);

                var sql = "UPDATE Contact "
                    + "SET ";

                var parameters = new DynamicParameters();
                if (existingEntity.FirstName != entityToUpdate.FirstName)
                {
                    sql += "FirstName=@FirstName,";
                    parameters.Add("@FirstName", entityToUpdate.FirstName, DbType.String);
                }

                if (existingEntity.LastName != entityToUpdate.LastName)
                {
                    sql += "LastName=@LastName,";
                    parameters.Add("@LastName", entityToUpdate.LastName, DbType.String);
                }

                if (existingEntity.Email != entityToUpdate.Email)
                {
                    sql += "Email=@Email,";
                    parameters.Add("@Email", entityToUpdate.Email, DbType.String);
                }

                if (existingEntity.Phone != entityToUpdate.Phone)
                {
                    sql += "Phone=@Phone,";
                    parameters.Add("@Phone", entityToUpdate.Phone, DbType.String);
                }

                if (existingEntity.Status != entityToUpdate.Status)
                {
                    sql += "Status=@Status,";
                    parameters.Add("@FirstName", entityToUpdate.Status, DbType.Boolean);
                }

                sql = sql.TrimEnd(',');

                sql += " WHERE Id=@Id";
                parameters.Add("@Id", entityToUpdate.Id, DbType.Int32);

                await conn.QueryAsync(sql, parameters);
            }
        }

        public Task<bool> MyCustomRepositoryMethodExampleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
