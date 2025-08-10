using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Transfer
{
    public class TransferCommandHandler(IAccountService accountService)
        : IRequestHandler<TransferCommand, Unit>
    {
        public async Task<Unit> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            await accountService.Transfer(request, cancellationToken);
            return Unit.Value;
        }
    }
}