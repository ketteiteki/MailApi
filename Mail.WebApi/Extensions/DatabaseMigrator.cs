using Dapper;
using FluentMigrator.Runner;
using Mail.Domain.Constants;
using Npgsql;

namespace Mail.WebApi.Extensions;

public static class DatabaseInitializer
{
    public static async Task CreateDatabaseAsync(this IServiceCollection serviceCollection)
    {
        var provider = serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider;
        var configuration = provider.GetRequiredService<IConfiguration>();
        var sqlConnectionString = configuration.GetConnectionString(AppSettingsConstants.SqlConnection);
        
        var connection = new NpgsqlConnection(sqlConnectionString);
        var doesDatabaseExist = await connection.ExecuteScalarAsync<bool>("select * from pg_database where datname = 'mail'");

        if (doesDatabaseExist) return;
        
        await connection.ExecuteAsync("create database mail;");
    }
    
    public static void MigrateDatabase(this IServiceProvider serviceProvider)
    {
        var provider = serviceProvider.CreateScope().ServiceProvider;
        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}