using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Mail.Application.BusinessLogic;
using Mail.Domain.Constants;
using Mail.Infrastructure.Interfaces;
using Mail.Infrastructure.Migrations;
using Mail.Infrastructure.Repositories;
using Mail.WebApi.DependencyInjection;
using Mail.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString(AppSettingsConstants.SqlDatabaseConnection);
var dbmsConnectionString = configuration.GetConnectionString(AppSettingsConstants.SqlDbmsConnection);

ArgumentException.ThrowIfNullOrEmpty(connectionString);
ArgumentException.ThrowIfNullOrEmpty(dbmsConnectionString);

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILetterRepository, LetterRepository>();

//services
builder.Services.AddScoped<LetterService>();

builder.Services.AddSingleton(connectionString);

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<IConventionSet>(_ => new DefaultConventionSet("public", ""));

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddPostgres();
        rb.WithGlobalConnectionString(connectionString);
        rb.ScanIn(typeof(Tables).Assembly).For.Migrations();
    });

builder.Services.AddAuthenticationServices(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.MigrateDatabase(dbmsConnectionString, connectionString);

app.Run();