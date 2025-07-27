using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.RegisterTransaction
{
    public class RegisterTransactionCommandHandler(IAccountService accountService)
        : IRequestHandler<RegisterTransactionCommand, bool>
    {
        public async Task<bool> Handle(RegisterTransactionCommand request, CancellationToken cancellationToken)
        {
            return await accountService.RegisterTransaction(request, cancellationToken);
        }
    }
}