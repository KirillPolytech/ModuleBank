using BankAccount.Features.Accounts.Delete;
using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Patch
{
    public class PatchAccountCommandHandler(IAccountService accountService)
        : IRequestHandler<DeleteAccountCommand, bool>
    {
        public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            return await accountService.Delete(request, cancellationToken);
        }
    }
}
