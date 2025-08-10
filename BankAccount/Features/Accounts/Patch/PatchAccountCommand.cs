using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.Patch
{
    public record PatchAccountCommand(Guid AccountId, PatchAccountDto AccountDto) : IRequest<Unit>;
}