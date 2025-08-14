using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.RegisterTransaction;

public class RegisterTransactionCommandHandler(IAccountService accountService)
    : IRequestHandler<RegisterTransactionCommand, Unit>
{
    public async Task<Unit> Handle(RegisterTransactionCommand request, CancellationToken cancellationToken)
    {
        await accountService.RegisterTransaction(request, cancellationToken);
        return Unit.Value;
    }
}