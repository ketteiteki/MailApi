using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task InsertUser(UserEntity userEntity);
}