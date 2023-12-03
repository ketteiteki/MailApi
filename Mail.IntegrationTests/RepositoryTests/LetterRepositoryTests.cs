#nullable disable

using Dapper;
using FluentAssertions;
using Mail.Domain.Entities;
using Xunit;

namespace Mail.IntegrationTests.RepositoryTests;

public class LetterRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetLetterByIdTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter1 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter2 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter3 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        await LetterRepository.InsertLetterAsync(letter1);
        var insertLetterResult = await LetterRepository.InsertLetterAsync(letter2);
        await LetterRepository.InsertLetterAsync(letter3);

        var getLetterByIdResult = await LetterRepository.GetLetterByIdAsync(insertLetterResult.Id);
        
        getLetterByIdResult.Should().NotBeNull();
        getLetterByIdResult.Id.Should().Be(insertLetterResult.Id);
        getLetterByIdResult.Title.Should().Be(insertLetterResult.Title);
        getLetterByIdResult.Content.Should().Be(insertLetterResult.Content);
        getLetterByIdResult.CreatedAt.Should().Be(insertLetterResult.CreatedAt);
    }

    [Fact]
    public async Task GetLetterWithOwnerByIdTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter1 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter2 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter3 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        await LetterRepository.InsertLetterAsync(letter1);
        var insertLetterResult = await LetterRepository.InsertLetterAsync(letter2);
        await LetterRepository.InsertLetterAsync(letter3);

        var getLetterByIdResult = await LetterRepository.GetLetterWithOwnerByIdAsync(insertLetterResult.Id);

        getLetterByIdResult.Should().NotBeNull();
        getLetterByIdResult.Id.Should().Be(insertLetterResult.Id);
        getLetterByIdResult.Title.Should().Be(insertLetterResult.Title);
        getLetterByIdResult.Content.Should().Be(insertLetterResult.Content);
        getLetterByIdResult.CreatedAt.Should().Be(insertLetterResult.CreatedAt);
        getLetterByIdResult.Owner.Should().NotBeNull();
        getLetterByIdResult.Owner.Id.Should().Be(letterOwnerResult.Id);
    }

    [Fact]
    public async Task GetLettersTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter1 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter2 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter3 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var insertLetter1Result = await LetterRepository.InsertLetterAsync(letter1);
        var insertLetter2Result = await LetterRepository.InsertLetterAsync(letter2);
        var insertLetter3Result = await LetterRepository.InsertLetterAsync(letter3);

        var getLettersWithOwnerResult = await LetterRepository.GetLettersAsync(letterReceiverResult.Id, 0, 10);

        var letterList = getLettersWithOwnerResult.ToList();
        var requestedLetter1 = letterList.First(x => x.Id == insertLetter1Result.Id);
        requestedLetter1.Title.Should().Be(insertLetter1Result.Title);
        requestedLetter1.Content.Should().Be(insertLetter1Result.Content);
        var requestedLetter2 = letterList.First(x => x.Id == insertLetter2Result.Id);
        requestedLetter2.Title.Should().Be(insertLetter2Result.Title);
        requestedLetter2.Content.Should().Be(insertLetter2Result.Content);
        var requestedLetter3 = letterList.First(x => x.Id == insertLetter3Result.Id);
        requestedLetter3.Title.Should().Be(insertLetter3Result.Title);
        requestedLetter3.Content.Should().Be(insertLetter3Result.Content);
    }
    
    [Fact]
    public async Task GetLettersWithOwnerTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter1 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter2 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var letter3 = new LetterEntity { 
            Title = Guid.NewGuid().ToString(), 
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult, 
            Receiver = letterReceiverResult
        };
        var insertLetter1Result = await LetterRepository.InsertLetterAsync(letter1);
        var insertLetter2Result = await LetterRepository.InsertLetterAsync(letter2);
        var insertLetter3Result = await LetterRepository.InsertLetterAsync(letter3);

        var getLettersWithOwnerResult = await LetterRepository.GetLettersWithOwnerAsync(letterReceiverResult.Id, 0, 10);

        var letterList = getLettersWithOwnerResult.ToList();
        var requestedLetter1 = letterList.First(x => x.Id == insertLetter1Result.Id);
        requestedLetter1.Title.Should().Be(insertLetter1Result.Title);
        requestedLetter1.Content.Should().Be(insertLetter1Result.Content);
        requestedLetter1.Owner.Should().NotBeNull();
        requestedLetter1.Owner.Id.Should().Be(letterOwnerResult.Id);
        var requestedLetter2 = letterList.First(x => x.Id == insertLetter2Result.Id);
        requestedLetter2.Title.Should().Be(insertLetter2Result.Title);
        requestedLetter2.Content.Should().Be(insertLetter2Result.Content);
        requestedLetter2.Owner.Should().NotBeNull();
        requestedLetter2.Owner.Id.Should().Be(letterOwnerResult.Id);
        var requestedLetter3 = letterList.First(x => x.Id == insertLetter3Result.Id);
        requestedLetter3.Title.Should().Be(insertLetter3Result.Title);
        requestedLetter3.Content.Should().Be(insertLetter3Result.Content);
        requestedLetter3.Owner.Should().NotBeNull();
        requestedLetter3.Owner.Id.Should().Be(letterOwnerResult.Id);
    }
    
    [Fact]
    public async Task InsertLetterTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter = new LetterEntity
        {
            Title = Guid.NewGuid().ToString(),
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult,
            Receiver = letterReceiverResult
        };
        
        var result = await LetterRepository.InsertLetterAsync(letter);

        await using var connection = GetConnection();
        var query = """
                    select * from "LetterEntity" l
                    left join "UserEntity" o on o."Id" = l."OwnerId"
                    left join "UserEntity" r on r."Id" = l."ReceiverId"
                    """;
        var requestedLetterData = await connection.QueryAsync<LetterEntity, UserEntity, UserEntity, LetterEntity>(query,
            (letterEntity, ownerEntity, receiverEntity) =>
            {
                letterEntity.Owner = ownerEntity;
                letterEntity.Receiver = receiverEntity;
                return letterEntity;
            });
        var requestedLetter = requestedLetterData.FirstOrDefault();
        requestedLetter.Should().NotBeNull();
        requestedLetter.Id.Should().Be(result.Id);
        requestedLetter.Title.Should().Be(result.Title);
        requestedLetter.Content.Should().Be(result.Content);
        requestedLetter.CreatedAt.Should().Be(result.CreatedAt);
    }
    
    [Fact]
    public async Task DeleteLetterTest()
    {
        var letterOwner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterReceiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var letterOwnerResult = await UserRepository.InsertUserAsync(letterOwner);
        var letterReceiverResult = await UserRepository.InsertUserAsync(letterReceiver);
        var letter = new LetterEntity
        {
            Title = Guid.NewGuid().ToString(),
            Content = Guid.NewGuid().ToString(),
            Owner = letterOwnerResult,
            Receiver = letterReceiverResult
        };
        var insertLetterResult = await LetterRepository.InsertLetterAsync(letter);
        var getLetterResultForCheck = await LetterRepository.GetLetterByIdAsync(insertLetterResult.Id);
        getLetterResultForCheck.Should().NotBeNull();
        getLetterResultForCheck.Id.Should().Be(insertLetterResult.Id);

        await LetterRepository.DeleteLetterAsync(insertLetterResult.Id);
        
        var getLetterResult = await LetterRepository.GetLetterByIdAsync(insertLetterResult.Id);
        getLetterResult.Should().BeNull();
    }
}