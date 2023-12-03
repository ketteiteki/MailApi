using Mail.Domain.Entities;

namespace Mail.Infrastructure.Interfaces;

public interface ILetterRepository
{
    Task<LetterEntity?> GetLetterByIdAsync(Guid letterId);
    
    Task<LetterEntity?> GetLetterWithOwnerByIdAsync(Guid letterId);
    
    Task<IEnumerable<LetterEntity>> GetLettersAsync(Guid userId, int offset, int limit);

    Task<IEnumerable<LetterEntity>> GetLettersWithOwnerAsync(Guid userId, int offset, int limit);
    
    Task<LetterEntity> InsertLetterAsync(LetterEntity letterEntity);

    Task<LetterEntity> DeleteLetterAsync(Guid letterId);
}