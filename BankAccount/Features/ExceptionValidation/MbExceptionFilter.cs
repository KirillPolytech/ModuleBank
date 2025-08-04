using BankAccount.Features.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace BankAccount.Features.ExceptionValidation
{
    public class MbExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<MbExceptionFilter> _logger;

        public MbExceptionFilter(ILogger<MbExceptionFilter> logger) => _logger = logger;

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            switch (context.Exception)
            {
                case JsonException:
                {
                    const string error = "Incorrect data format in the request body.";

                    context.Result = new JsonResult(MbResult<object>.Fail(error))
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    context.ExceptionHandled = true;
                    return;
                }
                case ValidationException validationException:
                {
                    var errorMessages = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.First().ErrorMessage
                        );

                    context.Result = new JsonResult(MbResult<object>.Fail("Validation failed", errorMessages))
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    context.ExceptionHandled = true;
                    return;
                }
                default:
                    context.Result = new JsonResult(MbResult<object>.Fail("Internal Server Error"))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}