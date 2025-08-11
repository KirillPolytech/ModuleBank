using BankAccount.Features.ExceptionValidation;
using FluentValidation;

namespace BankAccount.Features.Accounts.RegisterTransaction
{
    public class RegisterTransactionValidator : AbstractValidator<RegisterTransactionCommand>
    {
        public RegisterTransactionValidator()
        {
            RuleFor(x => x.TransactionDto.AccountId)
                .NotEmpty()
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.TransactionDto.AccountId)));

            RuleFor(x => x.TransactionDto.Amount)
                .NotEmpty()
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.TransactionDto.Amount)))
                .GreaterThan(0).
                WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.TransactionDto.Timestamp)
                .NotEmpty()
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.TransactionDto.Timestamp)))
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(ValidationMessages.TimestampCannotBeInTheFuture);

            RuleFor(x => x.TransactionDto.Description)
                .MaximumLength(500)
                .WithMessage("Description can't be longer than 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.TransactionDto.Description));
        }
    }
}
