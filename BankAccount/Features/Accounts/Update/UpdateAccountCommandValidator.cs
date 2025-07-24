using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Update
{
    public class UpdateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public UpdateAccountCommandValidator(IAccountService accountService)
        {
            RuleFor(x => x.AccountDto.OwnerId)
                .NotEmpty().WithMessage("OwnerId is required.")
                .MustAsync(async (ownerId, _) => await accountService.OwnerExistsAsync(ownerId))
                .WithMessage("OwnerId must exist.");

            /*
            RuleFor(x => x.AccountDto.Currency)
                .NotEmpty().WithMessage("CurrencyType is required.")
                .MustAsync(async (currency, _) => await accountService.IsCurrencySupportedAsync(currency))
                .WithMessage("CurrencyType is not supported.");
            */

            RuleFor(x => x.AccountDto.Type)
                .IsInEnum().WithMessage("Invalid account type.");

            RuleFor(x => x.AccountDto.InterestRate)
                .Must((cmd, interest) =>
                {
                    if (cmd.AccountDto.Type is AccountType.Deposit or AccountType.Credit)
                    {
                        return interest is >= 0;
                    }
                    return !interest.HasValue;
                })
                .WithMessage("InterestRate must be set (and non-negative) for Deposit and " +
                             "Credit accounts only, and must be null otherwise.");

            RuleFor(x => x.AccountDto.OpenDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("OpenDate cannot be in the future.");
        }
    }
}