using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.RegisterTransaction
{
    public record RegisterTransactionCommand(TransactionDto TransactionDto) : IRequest<bool>;
}