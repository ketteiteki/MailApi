namespace Mail.Application.Requests;

public record SendLetterRequest(Guid UserId, string Title, string Content);