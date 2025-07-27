using BankAccount.Features.Models.DTOs;
using BankAccount.Services.Interfaces;
using Mapster;
using MediatR;

namespace BankAccount.Features.Accounts.GetAccounts
{
    public class GetAccountsQueryHandler(IAccountService accountService)
        : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
    {
        public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            return (await accountService.GetAllByOwnerId(request.OwnerGuid, cancellationToken))
                .Adapt<IEnumerable<AccountDto>>();
        }
    }
}
