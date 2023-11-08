using FluentMigrator.Runner;
using Mail.Domain.Constants;
using Mail.Infrastructure;
using Mail.Infrastructure.Interfaces;
using Mail.Infrastructure.Migrations;
using Mail.Infrastructure.Repositories;
using Mail.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var sqlDatabaseConnectionString = configuration.GetConnectionString(AppSettingsConstants.SqlDatabaseConnection);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILetterRepository, LetterRepository>();

await builder.Services.CreateDatabaseAsync();

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddPostgres();
        rb.WithGlobalConnectionString(sqlDatabaseConnectionString);
        rb.ScanIn(typeof(Tables).Assembly).For.Migrations();
    });

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

app.UseAuthorization();

app.MapControllers();

app.Services.MigrateDatabase();

app.Run();