using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.GetAccounts
{
    public record GetAccountsQuery(Guid OwnerGuid) : IRequest<IEnumerable<AccountDto?>>;
}