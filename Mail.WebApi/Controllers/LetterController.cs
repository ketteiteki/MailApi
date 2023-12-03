using System.Security.Claims;
using Mail.Application.BusinessLogic;
using Mail.Application.Requests;
using Mail.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mail.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("Letters")]
public class LetterController(LetterService letterService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLetters([FromQuery] int offset, [FromQuery] int limit)
    {
        var requesterId = new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await letterService.GetLettersAsync(requesterId, offset, limit);
        
        return result.ToActionResult();
    }
    
    [HttpPost("Send")]
    public async Task<IActionResult> SendLetter(SendLetterRequest request)
    {
        var requesterId = new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await letterService.SendLetterAsync(requesterId, request.UserId, request.Title, request.Content);
        
        return result.ToActionResult();
    }
    
    [HttpDelete("{letterId:guid}")]
    public async Task<IActionResult> DeleteLetter(Guid letterId)
    {
        var requesterId = new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await letterService.DeleteLetterAsync(requesterId, letterId);
        
        return result.ToActionResult();
    }
}