using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Mail.Application.BusinessLogic;
using Mail.Infrastructure.Interfaces;
using Mail.Infrastructure.Migrations;
using Mail.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Mail.IntegrationTests.Configuration;

public static class MailStartup
{
    public static ServiceProvider Initialize(string connectionString)
    {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb.AddPostgres();
                rb.WithGlobalConnectionString(connectionString);
                rb.ScanIn(typeof(Tables).Assembly).For.Migrations();
            });

        serviceCollection.AddSingleton(connectionString);
        
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ILetterRepository, LetterRepository>();

        serviceCollection.AddScoped<LetterService>();
        
        serviceCollection.AddScoped<IConventionSet>(_ => new DefaultConventionSet("public", ""));
        
        return serviceCollection.BuildServiceProvider();
    }
}