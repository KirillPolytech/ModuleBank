using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.Patch
{
    public record PatchAccountCommand(Guid Guid, PatchAccountDto AccountDto) : IRequest<bool>;
}