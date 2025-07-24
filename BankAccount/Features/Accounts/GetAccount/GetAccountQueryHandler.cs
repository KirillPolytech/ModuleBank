using BankAccount.Features.Models.DTOs;
using BankAccount.Services.Interfaces;
using Mapster;
using MediatR;

namespace BankAccount.Features.Accounts.GetAccount
{
    public class GetAccountQueryHandler(IAccountService accountService)
        : IRequestHandler<GetAccountQuery, AccountDto?>
    {
        public async Task<AccountDto?> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await accountService.GetById(request.Guid, cancellationToken);
            return account.Adapt<AccountDto?>();
        }
    }
}