using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Patch
{
    public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
    {
        public PatchAccountCommandValidator(IAccountService accountService)
        {
        }
    }
}