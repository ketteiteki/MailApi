using Dapper;
using Mail.Domain.Entities;
using Mail.Infrastructure.Interfaces;
using Npgsql;

namespace Mail.Infrastructure.Repositories;

public class UserRepository(string connectionString) : IUserRepository
{
    public async Task<UserEntity?> GetUserById(Guid id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """select * from "UserEntity" where "Id" = @Id""";

        return await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new {Id = id});
    }
    
    public async Task InsertUser(UserEntity userEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var query = """insert into "UserEntity" ("Id",  "MailAddress") values (@Id, @MailAddress)""";

        await connection.ExecuteAsync(query, userEntity);
    }
}