using Dapper;
using Mail.Application.BusinessLogic;
using Mail.Domain.Constants;
using Mail.Infrastructure.Interfaces;
using Mail.IntegrationTests.Configuration;
using Mail.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

namespace Mail.IntegrationTests;

[Collection("Sequence")]
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    protected readonly IUserRepository UserRepository;
    protected readonly ILetterRepository LetterRepository;
    protected readonly LetterService LetterService;
    
    public IntegrationTestBase()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        
        ArgumentException.ThrowIfNullOrEmpty(databaseConnectionString);
        
        _serviceProvider = MailStartup.Initialize(databaseConnectionString);

        UserRepository = _serviceProvider.GetRequiredService<IUserRepository>();
        LetterRepository = _serviceProvider.GetRequiredService<ILetterRepository>();

        LetterService = _serviceProvider.GetRequiredService<LetterService>();
    }
    
    public async Task InitializeAsync()
    {
        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        var dbmsConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlDbmsConnection);
        
        ArgumentException.ThrowIfNullOrEmpty(databaseConnectionString);
        ArgumentException.ThrowIfNullOrEmpty(dbmsConnectionString);
        
        await _serviceProvider.MigrateDatabase(dbmsConnectionString, databaseConnectionString);

        await using var connection = new NpgsqlConnection(databaseConnectionString); 
        
        var query = """
                    truncate table "UserEntity" cascade;
                    truncate table "LetterEntity" cascade;
                    """;

        await connection.ExecuteAsync(query);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public NpgsqlConnection GetConnection()
    {
        var databaseConnectionString = _configuration.GetConnectionString(AppSettingsConstants.SqlIntegrationTestsDatabaseConnection);
        
        return new NpgsqlConnection(databaseConnectionString);
    }
}