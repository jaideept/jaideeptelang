using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
    class DbConnectionFactory
    {
        public static IDbConnection GetDbConnection(DbConnectionTypes dbType, string connectionString)
        {
            IDbConnection connection = null;

            switch (dbType)
            {
                case DbConnectionTypes.SQL:
                    connection = new SqlConnection(connectionString);
                    break;
                case DbConnectionTypes.XML:
                    // TODO: Implement XML Connection (path name)
                    break;
                case DbConnectionTypes.DOCUMENT:
                    // TODO: Implement Document DB connection
                    break;
                default:
                    connection = null;
                    break;
            }

            connection.Open();
            return connection;
        }
    }

    public enum DbConnectionTypes
    {
        SQL,
        DOCUMENT,
        XML
    }
}
