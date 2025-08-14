using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.CheckExists;

public class CheckAccountExistsQueryValidator : AbstractValidator<CheckAccountExistsQuery>
{
    public CheckAccountExistsQueryValidator(IAccountService accountService)
    {
        RuleFor(x => x.AccountGuid)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("AccountGuid must not be empty")
            .MustAsync(async (accountGuid, cancellationToken) =>
                await accountService.Exists(accountGuid, cancellationToken) == false)
            .WithMessage("AccountGuid must refer to an existing account");
    }
}