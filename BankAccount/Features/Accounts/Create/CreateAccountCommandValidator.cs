using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(IAccountService accountService)
        {
            RuleFor(x => x.AccountDto.OwnerId)
                .NotEmpty()
                .MustAsync(async (ownerId, _) => await accountService.OwnerExistsAsync(ownerId))
                .WithMessage("OwnerId must exist");

            /*
            RuleFor(x => x.AccountDto.Currency)
                .NotEmpty()
                .MustAsync(async (currency, _) => await accountService.IsCurrencySupportedAsync(currency))
                .WithMessage("CurrencyType is not supported");
            */

            RuleFor(x => x.AccountDto.Type)
                .IsInEnum();

            RuleFor(x => x.AccountDto.InterestRate)
                .Must((cmd, interest) =>
                {
                    if (cmd.AccountDto.Type is AccountType.Deposit or AccountType.Credit)
                        return interest.HasValue;
                    return interest == null;
                })
                .WithMessage("InterestRate must be set for Deposit and Credit accounts only");

            RuleFor(x => x.AccountDto.OpenDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("OpenDate cannot be in the future");
        }
    }
}