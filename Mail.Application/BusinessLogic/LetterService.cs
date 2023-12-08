using Mail.Application.DTOs;
using Mail.Domain.Constants;
using Mail.Domain.Entities;
using Mail.Domain.Responses;
using Mail.Domain.Responses.Errors;
using Mail.Infrastructure.Interfaces;

namespace Mail.Application.BusinessLogic;

public class LetterService(IUserRepository userRepository, ILetterRepository letterRepository)
{
    public async Task<Result<IEnumerable<LetterDto>>> GetLettersAsync(Guid requesterId, int offset, int limit)
    {
        var letters = await letterRepository.GetLettersWithOwnerAsync(requesterId, offset, limit);

        var lettersDto = letters.Select(LetterDto.Create);
        
        return new Result<IEnumerable<LetterDto>>(lettersDto);
    }
    
    public async Task<Result<LetterDto>> SendLetterAsync(Guid requesterId, Guid userId, string title, string content)
    {
        var requester = await userRepository.GetUserByIdAsync(requesterId);
        var user = await userRepository.GetUserByIdAsync(userId);

        if (requester == null)
        {
            return new Result<LetterDto>(new Error(ResponseMessages.RequesterNotFound));
        }
        
        if (user == null)
        {
            return new Result<LetterDto>(new Error(ResponseMessages.UserNotFound));
        }

        var newLetter = new LetterEntity
        {
            Owner = requester,
            Receiver = user,
            Title = title,
            Content = content
        };

        var createdLetter = await letterRepository.InsertLetterAsync(newLetter);

        var letter = await letterRepository.GetLetterWithOwnerByIdAsync(createdLetter.Id);

        if (letter == null)
        {
            return new Result<LetterDto>(new Error(ResponseMessages.CreatingLetterError));
        }
        
        var letterDto = LetterDto.Create(letter);
        
        return new Result<LetterDto>(letterDto);
    }
    
    public async Task<Result<LetterDto>> DeleteLetterAsync(Guid requesterId, Guid letterId)
    {
        var letter = await letterRepository.GetLetterWithOwnerByIdAsync(letterId);

        if (letter?.Owner?.Id != requesterId)
        {
            return new Result<LetterDto>(new Error(ResponseMessages.LetterNotFound));
        }

        await letterRepository.DeleteLetterAsync(letter.Id);

        var letterDto = LetterDto.Create(letter);
        
        return new Result<LetterDto>(letterDto);
    }
}