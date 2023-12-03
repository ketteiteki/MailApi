using Mail.Domain.Entities;

namespace Mail.Application.DTOs;

public class LetterDto
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    
    public required UserDto? Owner { get; set; }

    public static LetterDto Create(LetterEntity entity)
    {
        return new LetterDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
            Owner = entity.Owner != null ? UserDto.Create(entity.Owner) : null
        };
    }
}