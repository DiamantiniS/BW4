using System.Data.Common;
using System.Data.SqlClient;

namespace BW4.Services
{
    public class SQLServerServiceBase
    {
        private readonly SqlConnection _connection;

        public SQLServerServiceBase(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetConnectionString("BW4Conn"));
        }

        protected DbCommand getCommand(string commandText)
        {
            return new SqlCommand(commandText, _connection);
        }

        protected DbConnection getConnection()
        {
            return _connection;
        }
    }
}

