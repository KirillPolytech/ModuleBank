using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Transfer
{
    public class TransferCommandValidator : AbstractValidator<TransferCommand>
    {
        public TransferCommandValidator(IAccountService accountService)
        {
            RuleFor(x => x.TransferDto.Amount)
                .NotEqual(0)
                .WithMessage("Amount must greater than 0.");

            RuleFor(x => x.TransferDto.From)
                .NotEmpty()
                .WithMessage("From account is required.");

            RuleFor(x => x.TransferDto.To)
                .NotEmpty()
                .WithMessage("To account is required.");

            RuleFor(x => x.TransferDto)
                .NotEmpty()
                .DependentRules(() =>
            {
                RuleFor(x => x.TransferDto)
                    .MustAsync(async (dto, cancellationToken) =>
                    {
                        var fromAccount = await accountService.GetById(dto.From, cancellationToken);
                        var toAccount = await accountService.GetById(dto.To, cancellationToken);

                        if (fromAccount == null || toAccount == null)
                            return false;

                        return fromAccount.CurrencyType == toAccount.CurrencyType;
                    })
                    .WithMessage("Sender and recipient account currencies must match.");
            });

        }
    }
}