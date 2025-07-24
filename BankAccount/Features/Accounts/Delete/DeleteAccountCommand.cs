using MediatR;

namespace BankAccount.Features.Accounts.Delete
{
    public record DeleteAccountCommand(Guid Guid) : IRequest<bool>;
}