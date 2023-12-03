using Mail.Domain.Entities;

namespace Mail.Application.DTOs;

public class UserDto
{
    public required Guid Id { get; set; }

    public required string MailAddress { get; set; }

    public static UserDto Create(UserEntity entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            MailAddress = entity.MailAddress
        };
    }
}