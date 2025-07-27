using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.Update
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator(
            IAccountService accountService,
            IClientVerificationService clientVerificationService)
        {
            RuleFor(x => x.AccountDto.Id)
                .NotEmpty()
                .WithMessage("AccountId is required.")
                .MustAsync(async (accountGuid, cancellationToken)
                    => await accountService.GetById(accountGuid, cancellationToken) != null)
                .WithMessage("AccountId must exist.");


            RuleFor(x => x.AccountDto.OwnerId)
                .NotEmpty()
                .WithMessage("OwnerId is required.")
                .MustAsync(async (ownerId, cancellationToken) 
                    => await clientVerificationService.OwnerExistsAsync(ownerId, cancellationToken))
                .WithMessage("OwnerId must exist.");

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