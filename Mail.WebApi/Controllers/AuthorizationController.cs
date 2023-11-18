using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mail.WebApi.Controllers;

[ApiController]
[Route("")]
public class AuthorizationController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [Authorize]
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet("isAuthenticated")]
    public async Task<IActionResult> Check()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync();
        
        return Ok(authenticateResult.Succeeded);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var r = await HttpContext.AuthenticateAsync();

        if (r.Succeeded)
        {
            return Redirect("/isAuthenticated");
        }
        
        return Challenge();
    }
}