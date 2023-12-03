namespace Mail.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public required string MailAddress { get; set; }

    public List<LetterEntity> LetterEntities { get; set; } = new();
}