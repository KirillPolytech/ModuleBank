using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Delete
{
    public class DeleteAccountCommandHandler(IAccountService accountService)
        : IRequestHandler<DeleteAccountCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            await accountService.Delete(request, cancellationToken);
            return Unit.Value;
        }
    }
}