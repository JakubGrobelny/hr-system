using HrSystem.Application.Common.Exceptions;
using HrSystem.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace HrSystem.Api.ErrorHandling;

/// <summary>
/// Translates exceptions thrown by the application and domain layers into
/// RFC 9457 problem details responses.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            DomainException => (StatusCodes.Status422UnprocessableEntity, "Business rule violation"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred"),
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = statusCode,
                Title = title,
                // Never leak internals of unexpected errors to the client.
                Detail = statusCode == StatusCodes.Status500InternalServerError ? null : exception.Message,
            },
        });
    }
}
