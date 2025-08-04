using BankAccount.Features.Models;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace BankAccount.Features.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred");

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                validationEx.Errors.FirstOrDefault()?.ErrorMessage ?? "Validation failed"
            ),

            ApplicationException => (
                HttpStatusCode.BadRequest,
                "Application exception occurred."
            ),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "The request key not found."
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized."
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "Internal server error. Please retry later."
            )
        };

        var result = MbResult<object>.Fail(message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
    }
}