using System.Data;
using Mail.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Mail.Infrastructure;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(AppSettingsConstants.SqlDatabaseConnection);
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}