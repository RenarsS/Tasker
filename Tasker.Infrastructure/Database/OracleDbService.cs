using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Tasker.Infrastructure.Database;

public class OracleDbService(string? connectionString)
{
    public IDbConnection CreateConnection()
    {
        if (connectionString == null)
        {
            throw new NullReferenceException("Oracle connection string is not configured.");
        }
        
        return new OracleConnection(connectionString);
    }
}