using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.GetAccount
{
    public record GetAccountQuery(Guid Guid) : IRequest<AccountDto?>;
}