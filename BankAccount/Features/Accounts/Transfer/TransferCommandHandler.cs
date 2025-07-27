using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Transfer
{
    public class TransferCommandHandler(IAccountService accountService)
        : IRequestHandler<TransferCommand, bool>
    {
        public async Task<bool> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            return await accountService.Transfer(request, cancellationToken);
        }
    }
}