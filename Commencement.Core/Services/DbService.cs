using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Commencement.Core.Services
{
    public interface IDbService
    {
        IDbConnection GetConnection(string connectionString = null);
    }

    public class DbService : IDbService
    {
        /// <summary>
        /// Retry after 1 second and then every 2 seconds after that for 5 tries
        /// </summary>
        private static readonly RetryPolicy DefaultPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

        public IDbConnection GetConnection(string connectionString = null)
        {
            //If connection string is null, use the default sql ce connection
            if (connectionString == null)
            {
                connectionString = ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString;
            }

            var connection = new ReliableSqlConnection(connectionString, DefaultPolicy, DefaultPolicy);
            //var connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}