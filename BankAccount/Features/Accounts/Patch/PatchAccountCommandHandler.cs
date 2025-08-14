using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Patch;

public class PatchAccountCommandHandler(IAccountService accountService)
    : IRequestHandler<PatchAccountCommand, Unit>
{
    public async Task<Unit> Handle(PatchAccountCommand request, CancellationToken cancellationToken)
    {
        await accountService.Patch(request.AccountDto, cancellationToken);
        return Unit.Value;
    }
}