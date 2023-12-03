using Mail.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Mail.WebApi.Extensions;

public static class ResultExtension
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.Error != null)
        {
            return new ObjectResult(new { result.Error.Message }) { StatusCode = StatusCodes.Status400BadRequest };
        }

        return new ObjectResult(new { result.Response }) { StatusCode = StatusCodes.Status200OK};
    }
}