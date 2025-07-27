using FluentValidation;

namespace BankAccount.Features.Accounts.RegisterTransaction
{
    public class RegisterTransactionValidator : AbstractValidator<RegisterTransactionCommand>
    {
        public RegisterTransactionValidator()
        {
            RuleFor(x => x.TransactionDto.AccountId)
                .NotEmpty()
                .WithMessage("AccountId is required.");

            RuleFor(x => x.TransactionDto.Amount)
                .NotEmpty()
                .WithMessage("Amount is required.")
                .GreaterThan(0).
                WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.TransactionDto.AccountId)
                .IsInEnum()
                .WithMessage("Currency must be a valid enum value.");

            RuleFor(x => x.TransactionDto.AccountId)
                .IsInEnum()
                .WithMessage("Transaction Type must be a valid enum value.");

            RuleFor(x => x.TransactionDto.Timestamp)
                .NotEmpty()
                .WithMessage("Timestamp is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Timestamp cannot be in the future.");

            RuleFor(x => x.TransactionDto.Description)
                .MaximumLength(500)
                .WithMessage("Description can't be longer than 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.TransactionDto.Description));
        }
    }
}
