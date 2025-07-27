using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Patch
{
    public class PatchAccountCommandHandler(IAccountService accountService)
        : IRequestHandler<PatchAccountCommand, bool>
    {
        public async Task<bool> Handle(PatchAccountCommand request, CancellationToken cancellationToken)
        {
            return await accountService.Patch(request.AccountDto, cancellationToken);
        }
    }
}
