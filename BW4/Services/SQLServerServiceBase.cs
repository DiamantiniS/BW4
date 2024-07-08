using System.Data.Common;
using System.Data.SqlClient;

namespace BW4.Services
{
    public class SQLServerServiceBase
    {
        private SqlConnection _connection;


        public SQLServerServiceBase(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetConnectionString("BW4Conn"));
        }
        protected  DbCommand GetCommand(string commandText)
        {
            return new SqlCommand(commandText, _connection);
        }

        protected  DbConnection GetConnection()
        {
            return _connection;
        }
    }
}
