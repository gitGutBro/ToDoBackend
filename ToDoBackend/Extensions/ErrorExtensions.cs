using Microsoft.AspNetCore.Mvc;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToActionResult(this Error error)
    {
        if (error is null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        return error.Code switch
        {
            var code when code == ToDoErrors.MissingId.Code => new BadRequestObjectResult(error.Description),
            var code when code == ToDoErrors.NotFound.Code => new NotFoundObjectResult(error.Description),

            _ => new ObjectResult(new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = error.Description,
                Status = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}