using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using BankAccount.Services.Interfaces;
using Mapster;
using MediatR;

namespace BankAccount.Features.Accounts.Create;

public class CreateAccountCommandHandler(IAccountService accountService) 
    : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = request.AccountDto.Adapt<Account>();

        await accountService.Create(account, cancellationToken);
        return request.AccountDto;
    }
}