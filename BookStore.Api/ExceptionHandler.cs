using BookStore.Application.Common;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api
{
    public class ExceptionHandler(IProblemDetailsService problemDetails, ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            // Map exceptions to ProblemDetails
            ProblemDetails problem = exception switch
            {
                // Map FluentValidation exceptions to ValidationProblemDetails
                ValidationException validation => new ValidationProblemDetails(
                    validation.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest
                },

                // Map custom exceptions to ProblemDetails
                NotFoundException => new ProblemDetails { Status = StatusCodes.Status404NotFound, Detail = exception.Message },

                // Map ConflictException to ProblemDetails
                ConflictException => new ProblemDetails { Status = StatusCodes.Status409Conflict, Detail = exception.Message },

                // Map OpenLibraryUnavailableException to ProblemDetails
                OpenLibraryUnavailableException => new ProblemDetails { Status = StatusCodes.Status503ServiceUnavailable, Detail = exception.Message },

                // Default case for unhandled exceptions
                _ => new ProblemDetails { Status = StatusCodes.Status500InternalServerError, Detail = "Something went wrong." }
            };

            // Log unhandled exceptions
            if (problem.Status == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled error for {Method} {Path}", context.Request.Method, context.Request.Path);

            // Set the response status code and write the ProblemDetails to the response
            context.Response.StatusCode = problem.Status!.Value;

            // Write the ProblemDetails to the response using the IProblemDetailsService
            return await problemDetails.TryWriteAsync(new ProblemDetailsContext { HttpContext = context, ProblemDetails = problem });

        }
    }
}
