using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Delete
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator(IAccountService accountService)
        {
            RuleFor(x => x.AccountGuid)
                .NotEmpty().WithMessage("Account ID must not be empty")
                .MustAsync(async (accountId, cancellation) =>
                    await accountService.HasAccount(accountId, cancellation))
                .WithMessage("Account with the given ID does not exist");
        }
    }
}