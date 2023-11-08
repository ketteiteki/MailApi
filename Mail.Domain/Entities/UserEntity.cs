namespace Mail.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string MailAddress { get; set; }

    public IEnumerable<LetterEntity> LetterEntities { get; set; } = new List<LetterEntity>();
    
    public UserEntity(string mailAddress)
    {
        MailAddress = mailAddress;
    }
}