using BankAccount.Features.Models;
using BankAccount.Services.Interfaces;
using Mapster;
using MediatR;

namespace BankAccount.Features.Accounts.Update
{
    public class UpdateAccountCommandHandler(IAccountService accountService) 
        : IRequestHandler<UpdateAccountCommand, bool>
    {
        public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = request.AccountDto.Adapt<Account>();

            return await accountService.Update(account, cancellationToken); ;
        }
    }
}