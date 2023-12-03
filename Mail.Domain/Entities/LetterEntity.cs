namespace Mail.Domain.Entities;

public class LetterEntity
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    
    public required UserEntity? Receiver { get; set; }
    
    public required UserEntity? Owner { get; set; }
}