using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using BankAccount.Services.Interfaces;
using Mapster;
using MediatR;

namespace BankAccount.Features.Accounts.Update
{
    public class UpdateAccountCommandHandler(IAccountService accountService) 
        : IRequestHandler<UpdateAccountCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = request.AccountDto.Adapt<Account>();
            await accountService.Patch(account.Adapt<PatchAccountDto>(), cancellationToken);
            return Unit.Value;
        }
    }
}