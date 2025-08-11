using BankAccount.Features.ExceptionValidation;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Delete
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator(IAccountService accountService)
        {
            RuleFor(x => x.AccountGuid)
                .NotEmpty()
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.AccountGuid)))
                .MustAsync(async (accountId, cancellation) =>
                    await accountService.HasAccount(accountId, cancellation))
                .WithMessage(ValidationMessages.AccountNotFound);
        }
    }
}