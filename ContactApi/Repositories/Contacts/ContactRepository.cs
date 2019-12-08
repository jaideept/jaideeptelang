using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess;
using ContactApi.Models;

namespace ContactApi.Repository
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

        public async Task<IEnumerable<Contact>> GetByPageIndex<Contact>(int page, int pageSize)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = @"SELECT * FROM Contact ORDER BY Id OFFSET((@PageIndex-1)*@PageSize) ROWS
	                    FETCH NEXT @PageSize ROWS ONLY;";

                var param = new DynamicParameters();
                param.Add("@PageIndex", page);
                param.Add("@PageSize", pageSize);
                var list = await conn.QueryAsync<Contact>(sql, param, commandType: CommandType.Text);
                return list;
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

        public override async Task<Contact> InsertAsync(Contact entity)
        {
            StringBuilder sql = new StringBuilder();
            int newContactId;

            using (var conn = GetOpenConnection())
            {
                var sqlStatement = @"INSERT INTO Contact(FirstName, LastName, Email, Phone, Status) 
                    VALUES (@FirstName, @LastName, @Email, @Phone, @Status);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                newContactId = await conn.ExecuteScalarAsync<int>(sqlStatement, entity);

                entity.Id = newContactId;
            }

            return entity;
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

                sql += " WHERE Id = @Id";
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
