using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.GetAccount
{
    public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
    {
        public GetAccountQueryValidator(IClientVerificationService clientVerificationService)
        {
            RuleFor(x => x.AccountGuid)
                .NotEmpty()
                .MustAsync(async (ownerId, cancellationToken) 
                    => await clientVerificationService.OwnerExistsAsync(ownerId, cancellationToken))
                .WithMessage("OwnerId must exist");
        }
    }
}