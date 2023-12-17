using Mail.Application.DTOs;
using Mail.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mail.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("Users")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var users = await userRepository.GetUsersAsync(offset, limit);

        var usersDto = users.Select(UserDto.Create);
        
        return Ok(usersDto);
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUsers(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return Ok(null);
        
        var userDto = UserDto.Create(user);
        
        return Ok(userDto);
    }
}