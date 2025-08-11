using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.CheckExists
{
    public class CheckAccountExistsQueryHandler(IAccountService accountService)
        : IRequestHandler<CheckAccountExistsQuery, bool>
    {
        public async Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
        {
            return await accountService.Exists(request.AccountGuid, cancellationToken);
        }
    }
}