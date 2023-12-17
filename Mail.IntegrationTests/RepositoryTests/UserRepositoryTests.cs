#nullable disable

using FluentAssertions;
using Mail.Domain.Entities;
using Xunit;

namespace Mail.IntegrationTests.RepositoryTests;

public class UserRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task InsertUserTest()
    {
        var user = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };

        var result = await UserRepository.InsertUserAsync(user);

        var requestedUser = await UserRepository.GetUserByIdAsync(result.Id);
        requestedUser.Should().NotBeNull();
        requestedUser?.Id.Should().Be(result.Id);
        requestedUser?.MailAddress.Should().Be(result.MailAddress);
    }
    
    [Fact]
    public async Task GetUsersTest()
    {
        var user1 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var user2 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var user3 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        await UserRepository.InsertUserAsync(user1);
        await UserRepository.InsertUserAsync(user2);
        await UserRepository.InsertUserAsync(user3);

        var requestedUsers = await UserRepository.GetUsersAsync(0, 10);

        var userList = requestedUsers.ToList();
        userList.Count.Should().Be(3);
        userList.First(x => x.Id == user1.Id).Should().NotBeNull();
        userList.First(x => x.Id == user2.Id).Should().NotBeNull();
        userList.First(x => x.Id == user3.Id).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetUserByIdTest()
    {
        var user1 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var user2 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        var user3 = new UserEntity { Id = Guid.NewGuid(), MailAddress = Guid.NewGuid().ToString() };
        await UserRepository.InsertUserAsync(user1);
        var result = await UserRepository.InsertUserAsync(user2);
        await UserRepository.InsertUserAsync(user3);

        var requestedUser2 = await UserRepository.GetUserByIdAsync(result.Id);
        
        requestedUser2.Should().NotBeNull();
        requestedUser2?.Id.Should().Be(result.Id);
        requestedUser2?.MailAddress.Should().Be(result.MailAddress);
    }
}