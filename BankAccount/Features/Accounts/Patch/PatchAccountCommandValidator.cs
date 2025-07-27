using FluentValidation;

namespace BankAccount.Features.Accounts.Patch
{
    public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
    {
        public PatchAccountCommandValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId must exist");
        }
    }
}