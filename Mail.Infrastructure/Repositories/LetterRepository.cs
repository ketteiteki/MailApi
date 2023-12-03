using Dapper;
using Mail.Domain.Entities;
using Mail.Infrastructure.Interfaces;
using Npgsql;

namespace Mail.Infrastructure.Repositories;

public class LetterRepository(string connectionString) : ILetterRepository
{
    public async Task<LetterEntity?> GetLetterByIdAsync(Guid letterId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "LetterEntity"
                    where "Id" = @Id
                    limit 1
                    """;

        var param = new { Id = letterId };

        return await connection.QueryFirstOrDefaultAsync<LetterEntity>(query, param);
    }

    public async Task<LetterEntity?> GetLetterWithOwnerByIdAsync(Guid letterId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "LetterEntity" l
                    left join "UserEntity" u on u."Id" = l."OwnerId"
                    where l."Id" = @LetterId
                    limit 1
                    """;

        var param = new { LetterId = letterId };

        var letterData = await connection.QueryAsync<LetterEntity, UserEntity, LetterEntity>(query, (letter, user) =>
        {
            letter.Owner = user;
            return letter;
        }, param);
        
        return letterData.FirstOrDefault();
    }

    public async Task<IEnumerable<LetterEntity>> GetLettersAsync(Guid userId, int offset, int limit)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "LetterEntity"
                    where "ReceiverId" = @ReceiverId
                    order by "CreatedAt" DESC
                    offset @Offset limit @Limit
                    """;
        
        var param = new
        {
            ReceiverId = userId,
            Offset = offset,
            Limit = limit
        };

        return await connection.QueryAsync<LetterEntity>(query, param);
    }
    
    public async Task<IEnumerable<LetterEntity>> GetLettersWithOwnerAsync(Guid userId, int offset, int limit)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "LetterEntity" l
                    left join "UserEntity" u on u."Id" = l."OwnerId" 
                    where "ReceiverId" = @ReceiverId
                    order by "CreatedAt" DESC
                    offset @Offset limit @Limit
                    """;
        
        var param = new
        {
            ReceiverId = userId,
            Offset = offset,
            Limit = limit
        };

        return await connection.QueryAsync<LetterEntity, UserEntity, LetterEntity>(query, (letter, user) =>
        {
            letter.Owner = user;
            return letter;
        }, param);
    }


    public async Task<LetterEntity> InsertLetterAsync(LetterEntity letterEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    insert into "LetterEntity" ("Title", "Content", "OwnerId", "ReceiverId") 
                    values (@Title, @Content, @OwnerId, @ReceiverId)
                    returning *
                    """;

        var param = new
        {
            letterEntity.Title,
            letterEntity.Content,
            OwnerId = letterEntity.Owner?.Id,
            ReceiverId = letterEntity.Receiver?.Id
        };
        
        return await connection.QueryFirstAsync<LetterEntity>(query, param);
    }

    public async Task<LetterEntity> DeleteLetterAsync(Guid letterId)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    delete from "LetterEntity" 
                    where "Id" = @LetterId 
                    returning *
                    """;

        var param = new { LetterId = letterId };
        
        return await connection.QueryFirstAsync<LetterEntity>(query, param);
    }
}