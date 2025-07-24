using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.Create
{
    public record CreateAccountCommand(AccountDto AccountDto) : IRequest<AccountDto>;
}