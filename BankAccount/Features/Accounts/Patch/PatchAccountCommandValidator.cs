using BankAccount.Features.ExceptionValidation;
using FluentValidation;

namespace BankAccount.Features.Accounts.Patch;

public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
{
    public PatchAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage(x => ValidationMessages.RequiredField(nameof(x.AccountId)));
    }
}