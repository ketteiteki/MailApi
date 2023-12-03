using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Mail.WebApi.Extensions;

public static class DatabaseInitializer
{
    public static async Task MigrateDatabase(this IServiceProvider serviceProvider, string dbmsConnectionString, string databaseConnectionString)
    {
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(databaseConnectionString);

        if (!sqlConnectionStringBuilder.TryGetValue("Database", out var databaseName))
        {
            throw new Exception("Incorrect database connection string");
        } 
        
        var connection = new NpgsqlConnection(dbmsConnectionString);
        var doesDatabaseExist = await connection.ExecuteScalarAsync<bool>($"""select exists(select * from pg_database where datname = '{databaseName}');""");

        if (doesDatabaseExist) return;
        
        await connection.ExecuteAsync($"""create database "{databaseName}";""");

        var databaseConnection = new NpgsqlConnection(databaseConnectionString);
        
        await databaseConnection.ExecuteAsync("""create extension if not exists "uuid-ossp";""");
        
        var provider = serviceProvider.CreateScope().ServiceProvider;
        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}