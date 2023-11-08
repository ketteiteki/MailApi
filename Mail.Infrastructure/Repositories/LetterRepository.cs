using Mail.Domain.Entities;
using Mail.Infrastructure.Interfaces;

namespace Mail.Infrastructure.Repositories;

public class LetterRepository : ILetterRepository
{
    public async Task<IEnumerable<LetterEntity>> GetLetters(Guid userId, int page, int count)
    {
        throw new NotImplementedException();
    }

    public async Task InsertLetter(LetterEntity letterEntity)
    {
        throw new NotImplementedException();
    }
}