using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DnD.Api.Exceptions
{
   
    public class GlobalExceptionHandler(IHostEnvironment environment, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly IHostEnvironment _environment = environment;
        private readonly ILogger<GlobalExceptionHandler> _logger = logger; 

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
           
            _logger.LogError(exception, "Eccezione non gestita catturata: {ErrorMessage}", exception.Message);

            var details = new ProblemDetails
            {
                Detail = _environment.IsDevelopment() ? exception.Message : "Oops!! Something went wrong!",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error"
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            return true;
        }
    }
}