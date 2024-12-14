using System.Net;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToProblem(this Error error)
    {
        return CreateProblemResult(error);
    }

    
    public static ActionResult<T> ToProblem<T>(this Error error)
    {
        return CreateProblemResult(error);
    }
    
    private static ObjectResult CreateProblemResult(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => new ObjectResult(error.Description) { StatusCode = (int)HttpStatusCode.NotFound },
            ErrorType.Validation => new ObjectResult(error.Description) { StatusCode = (int)HttpStatusCode.BadRequest },
            _ => new ObjectResult("An unexpected error occurred.") { StatusCode = (int)HttpStatusCode.InternalServerError }
        };
    }
}