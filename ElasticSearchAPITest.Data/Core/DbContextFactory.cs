using ElasticSearchAPITest.Data.Interfaces;
using ElasticSearchAPITest.Data.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace ElasticSearchAPITest.Data.Core
{
    public class DbContextFactory : IDbContextFactory
    {
        #region Member

        private readonly IOptions<MySQLConnectionConfig> _mySQLConfig;

        #endregion

        public DbContextFactory(IOptions<MySQLConnectionConfig> config)
        {
            _mySQLConfig = config;
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_mySQLConfig.Value.DefaultConnection);
        }
    }
}
