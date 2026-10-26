using Microsoft.AspNetCore.Diagnostics;

namespace WebAppAgroAPI.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger  = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = exception switch
            {
                EntityNotFoundException => StatusCodes.Status404NotFound,

                EntityAlreadyExistsException => StatusCodes.Status409Conflict,

                EntityNotAuthorizedException => StatusCodes.Status403Forbidden,

                ArgumentException => StatusCodes.Status400BadRequest,

                _ => StatusCodes.Status500InternalServerError
            };

            //log exception

            if (statusCode >= 500)
            {
                _logger.LogError(exception, "An exception occured. StatusCode: {StatusCode}", statusCode);
            }

            else
            {
                _logger.LogWarning(exception, "Request failed. StatusCode: {StatusCode}", statusCode);
            }

                httpContext.Response.StatusCode = statusCode;

            var response = new
            {
                Status = statusCode,
                Message = statusCode == 500
                    ? "An unexpected error occured."
                    : exception.Message
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
