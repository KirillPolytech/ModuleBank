using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator(
            IClientVerificationService clientVerificationService,
            ICurrencyService currencyService)
        {
            RuleFor(x => x.AccountDto.OwnerId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("OwnerId must not be empty");

            RuleFor(x => x.AccountDto.OwnerId)
                .MustAsync(async (ownerId, cancellation) =>
                    await clientVerificationService.OwnerExistsAsync(ownerId, cancellation))
                .WithMessage("OwnerId must refer to an existing owner");

            RuleFor(x => x.AccountDto.Currency)
                .MustAsync(async (currency, cancellation) =>
                    await currencyService.IsCurrencySupported(currency, cancellation))
                .WithMessage("CurrencyType is not supported");

            RuleFor(x => x.AccountDto.Type)
                .IsInEnum()
                .WithMessage("Account type is not valid");

            RuleFor(x => x.AccountDto.InterestRate)
                .Must((cmd, interest) =>
                {
                    var type = cmd.AccountDto.Type;
                    return type is AccountType.Deposit or AccountType.Credit
                        ? interest.HasValue
                        : interest is null or 0;
                })
                .WithMessage("InterestRate must be set only for Deposit and Credit accounts");

            RuleFor(x => x.AccountDto.OpenDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("OpenDate cannot be in the future");
        }
    }
}