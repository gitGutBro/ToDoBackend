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
                ErrorCode.MissingId => new BadRequestObjectResult(error.Description),
                ErrorCode.NotFound => new NotFoundObjectResult(error.Description),
                ErrorCode.ValidationError => new BadRequestObjectResult(error.Description),
                ErrorCode.DatabaseError => new ObjectResult(new ProblemDetails
                {
                    Title = "Database Error",
                    Detail = error.Description,
                    Status = StatusCodes.Status500InternalServerError
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError 
                },
                ErrorCode.UnknownError => new ObjectResult(new ProblemDetails
                {
                    Title = "Unknown Error",
                    Detail = error.Description,
                    Status = StatusCodes.Status500InternalServerError
                })
                { 
                    StatusCode = StatusCodes.Status500InternalServerError 
                },

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