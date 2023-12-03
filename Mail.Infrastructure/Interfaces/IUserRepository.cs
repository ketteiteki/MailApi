using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetUserByIdAsync(Guid id);
    
    Task<UserEntity> InsertUserAsync(UserEntity userEntity);
}