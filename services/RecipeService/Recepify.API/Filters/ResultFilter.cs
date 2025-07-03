using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recepify.Core.ResultPattern;

namespace Recepify.API.Filters;

public class ResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is IResultBase resultBase)
        {
            if (resultBase.IsSuccess)
            {
                context.Result = new OkObjectResult(resultBase.GetValue());
            }
            else
            {
                var error = resultBase.Error ?? Error.None; // Ensure error is not null
                var statusCode = error.StatusCode;
                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = error.Title ?? "Error",
                    Detail = error.Description ?? "An error occurred.",
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = statusCode
                };
            }
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        // No action needed after the result has been executed
    }
}
