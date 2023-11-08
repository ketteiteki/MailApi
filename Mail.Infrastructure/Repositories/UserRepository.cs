using Mail.Domain.Entities;
using Mail.Infrastructure.Interfaces;

namespace Mail.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _dapperContext;

    public UserRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task InsertUser(UserEntity userEntity)
    {
        throw new NotImplementedException();
    }
}