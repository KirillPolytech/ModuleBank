using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Create;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(
        IClientVerificationService clientVerificationService,
        ICurrencyService currencyService)
    {
        RuleFor(x => x.AccountDto.OwnerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(x => ValidationMessages.RequiredField(nameof(x.AccountDto.OwnerId)));

        RuleFor(x => x.AccountDto.OwnerId)
            .MustAsync(async (ownerId, cancellation) =>
                await clientVerificationService.OwnerExistsAsync(ownerId, cancellation))
            .WithMessage(ValidationMessages.OwnerIdMustReferToAnExistingOwner);

        RuleFor(x => x.AccountDto.Currency)
            .MustAsync(async (currency, cancellation) =>
                await currencyService.IsCurrencySupported(currency, cancellation))
            .WithMessage(ValidationMessages.CurrencyTypeNotSupported);

        RuleFor(x => x.AccountDto.Type)
            .IsInEnum()
            .WithMessage(ValidationMessages.AccountTypeIsNotValid);

        RuleFor(x => x.AccountDto.InterestRate)
            .Must((cmd, interest) =>
            {
                var type = cmd.AccountDto.Type;
                return type is AccountType.Deposit or AccountType.Credit
                    ? interest.HasValue
                    : interest is null or 0;
            })
            .WithMessage(ValidationMessages.InterestRateMustBeSetOnlyForDepositAndCreditAccounts);

        RuleFor(x => x.AccountDto.OpenDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ValidationMessages.OpenDateCannotBeInTheFuture);
    }
}