using FluentMigrator.Runner;
using Mail.Domain.Constants;
using Mail.Infrastructure;
using Mail.Infrastructure.Interfaces;
using Mail.Infrastructure.Migrations;
using Mail.Infrastructure.Repositories;
using Mail.WebApi.DependencyInjection;
using Mail.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var sqlDatabaseConnectionString = configuration.GetConnectionString(AppSettingsConstants.SqlDatabaseConnection);

//db context
builder.Services.AddSingleton<DapperContext>();

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILetterRepository, LetterRepository>();

builder.Services.AddScoped<HttpClient>();

await builder.Services.CreateDatabaseAsync();

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddPostgres();
        rb.WithGlobalConnectionString(sqlDatabaseConnectionString);
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

app.Services.MigrateDatabase();

app.Run();