#nullable disable

using Dapper;
using FluentAssertions;
using Mail.Domain.Entities;
using Mail.Domain.Responses.Errors;
using Xunit;

namespace Mail.IntegrationTests.BusinessLogicServiceTests;

public class LetterServiceTest : IntegrationTestBase
{
    [Fact]
    public async Task GetLettersTest_Success()
    {
        var owner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var receiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var insertOwnerResult = await UserRepository.InsertUserAsync(owner);
        var insertReceiverResult = await UserRepository.InsertUserAsync(receiver);
        for (int i = 0; i < 10; i++)
        {
            await LetterService.SendLetterAsync(
                insertOwnerResult.Id,
                insertReceiverResult.Id,
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());
        }

        var getThreeLettersResult = await LetterService.GetLettersAsync(insertReceiverResult.Id, 0, 3);
        var getAllLettersResult = await LetterService.GetLettersAsync(insertReceiverResult.Id, 0, 100);

        getThreeLettersResult.Response.Count().Should().Be(3);
        getAllLettersResult.Response.Count().Should().Be(10);
    }
    
    [Fact]
    public async Task SendLetterTest_Success()
    {
        var owner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var receiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var insertOwnerResult = await UserRepository.InsertUserAsync(owner);
        var insertReceiverResult = await UserRepository.InsertUserAsync(receiver);
        var letterTitle = Guid.NewGuid().ToString();
        var letterContent = Guid.NewGuid().ToString();

        var sendLetterResult = await LetterService.SendLetterAsync(
            insertOwnerResult.Id,
            insertReceiverResult.Id,
            letterTitle,
            letterContent);

        await using var connection = GetConnection();
        var query = $"""
                    select * from "LetterEntity"
                    where "Id" = '{sendLetterResult.Response.Id}' and "OwnerId" = '{insertOwnerResult.Id}' and "ReceiverId" = '{insertReceiverResult.Id}'
                    """;
        var letter = await connection.QueryFirstAsync<LetterEntity>(query);
        letter.Title.Should().Be(letterTitle);
        letter.Content.Should().Be(letterContent);
    }
    
    [Fact]
    public async Task SendLetterTest_ThrowUserNotFound()
    {
        var owner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var receiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var insertOwnerResult = await UserRepository.InsertUserAsync(owner);
        var insertReceiverResult = await UserRepository.InsertUserAsync(receiver);
        var letterTitle = Guid.NewGuid().ToString();
        var letterContent = Guid.NewGuid().ToString();

        var sendLetterWithoutOwnerResult = await LetterService.SendLetterAsync(
            Guid.NewGuid(),
            insertReceiverResult.Id,
            letterTitle,
            letterContent);
        var sendLetterWithoutReceiverResult = await LetterService.SendLetterAsync(
            insertOwnerResult.Id,
            Guid.NewGuid(),
            letterTitle,
            letterContent);

        sendLetterWithoutOwnerResult.Error.Should().BeOfType<Error>();
        sendLetterWithoutReceiverResult.Error.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task DeleteLetterTest_Success()
    {
        var owner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var receiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var insertOwnerResult = await UserRepository.InsertUserAsync(owner);
        var insertReceiverResult = await UserRepository.InsertUserAsync(receiver);
        var letterTitle = Guid.NewGuid().ToString();
        var letterContent = Guid.NewGuid().ToString();
        var sendLetterResult = await LetterService.SendLetterAsync(
            insertOwnerResult.Id,
            insertReceiverResult.Id,
            letterTitle,
            letterContent);
        var getLetterResultForCheck = await LetterRepository.GetLetterByIdAsync(sendLetterResult.Response.Id);
        getLetterResultForCheck.Should().NotBeNull();

        await LetterService.DeleteLetterAsync(insertOwnerResult.Id, sendLetterResult.Response.Id);
        
        var getLetterResult = await LetterRepository.GetLetterByIdAsync(sendLetterResult.Response.Id);
        getLetterResult.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteLetterTest_ThrowLetterNotFound()
    {
        var owner = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var receiver = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var insertOwnerResult = await UserRepository.InsertUserAsync(owner);
        var insertReceiverResult = await UserRepository.InsertUserAsync(receiver);
        var letterTitle = Guid.NewGuid().ToString();
        var letterContent = Guid.NewGuid().ToString();
        var sendLetterResult = await LetterService.SendLetterAsync(
            insertOwnerResult.Id,
            insertReceiverResult.Id,
            letterTitle,
            letterContent);
        var getLetterResultForCheck = await LetterRepository.GetLetterByIdAsync(sendLetterResult.Response.Id);
        getLetterResultForCheck.Should().NotBeNull();

        var deleteLetterWithoutOwnerResult = await LetterService.DeleteLetterAsync(Guid.NewGuid(), sendLetterResult.Response.Id);
        var deleteLetterWithoutLetterResult = await LetterService.DeleteLetterAsync(insertOwnerResult.Id, Guid.NewGuid());

        deleteLetterWithoutOwnerResult.Error.Should().BeOfType<Error>();
        deleteLetterWithoutLetterResult.Error.Should().BeOfType<Error>();
    }
}