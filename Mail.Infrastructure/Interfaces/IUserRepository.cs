using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetUserByIdAsync(Guid id);

    Task<IEnumerable<UserEntity>> GetUsersAsync(int offset, int count);
    
    Task<UserEntity> InsertUserAsync(UserEntity userEntity);
}