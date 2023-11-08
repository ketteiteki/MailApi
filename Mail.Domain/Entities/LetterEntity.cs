using Mail.Domain.Enum;

namespace Mail.Domain.Entities;

public class LetterEntity
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public UserEntity Owner { get; set; }

    public LetterType Type { get; set; }

    public LetterEntity(string title, string content, UserEntity owner, LetterType type)
    {
        Title = title;
        Content = content;
        Owner = owner;
        Type = type;
    }
}