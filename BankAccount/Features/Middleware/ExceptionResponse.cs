using System.Net;

namespace BankAccount.Features.Middleware
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
}