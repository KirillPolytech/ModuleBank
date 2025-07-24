using BankAccount.Features.Models;
using MediatR;

namespace BankAccount.Features.Accounts.GetStatement
{
    public record GetStatementQuery(Guid AccountId, DateTime? From, DateTime? To) : IRequest<IEnumerable<Transaction?>>;
}