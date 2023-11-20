using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetUserById(Guid id);
    
    Task InsertUser(UserEntity userEntity);
}