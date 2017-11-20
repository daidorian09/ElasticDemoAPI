using MySql.Data.MySqlClient;

namespace ElasticSearchAPITest.Data.Interfaces
{
    public interface IDbContextFactory
    {
        MySqlConnection CreateConnection();
    }
}
