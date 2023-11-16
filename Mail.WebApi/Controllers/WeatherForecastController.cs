using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mail.WebApi.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

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
    
    [HttpGet("check")]
    public async Task<IActionResult> Check()
    {
        var r = await HttpContext.AuthenticateAsync();
        var ticket = r.Ticket;
        var accessToken = r.Ticket?.Properties.GetTokenValue("access_token");
        
        return Ok(1);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var r = await HttpContext.AuthenticateAsync();

        if (r.Succeeded)
        {
            return Redirect("/check");
        }
        
        return Challenge();
    }
}