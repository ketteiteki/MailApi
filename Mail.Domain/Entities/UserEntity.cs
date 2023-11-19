namespace Mail.Domain.Entities;

public class UserEntity(Guid id, string mailAddress)
{
    public Guid Id { get; set; } = id;

    public string MailAddress { get; set; } = mailAddress;

    public IEnumerable<LetterEntity> LetterEntities { get; set; } = new List<LetterEntity>();
}