using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankAccount.Features.ExceptionValidation
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not ValidationException validationException) 
                return;

            context.Result = new BadRequestObjectResult(new
            {
                Errors = validationException
                    .Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });

            context.ExceptionHandled = true;
        }
    }
}