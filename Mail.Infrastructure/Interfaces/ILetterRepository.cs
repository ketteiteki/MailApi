using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface ILetterRepository
{
    Task<IEnumerable<LetterEntity>> GetLetters(Guid userId, int page, int count);

    Task InsertLetter(LetterEntity letterEntity);
}