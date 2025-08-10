using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.Transfer
{
    public record TransferCommand(TransferDto TransferDto) : IRequest<Unit>;
}