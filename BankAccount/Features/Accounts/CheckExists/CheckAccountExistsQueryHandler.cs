using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.CheckExists
{
    public class CheckAccountExistsQueryHandler(IAccountService accountService)
        : IRequestHandler<CheckAccountExistsQuery, bool>
    {
        public Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(accountService.GetById(request.AccountGuid, cancellationToken).Result is not null);
        }
    }
}