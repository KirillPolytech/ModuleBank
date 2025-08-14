using BankAccount.Features.ExceptionValidation;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.GetAccounts;

public class GetAccountsValidator : AbstractValidator<GetAccountsQuery>
{
    public GetAccountsValidator(IClientVerificationService clientVerificationService)
    {
        RuleFor(x => x)
            .NotEmpty()
            .MustAsync(async (getAccountsQuery, cancellationToken) 
                => await clientVerificationService.OwnerExistsAsync(getAccountsQuery.OwnerGuid, cancellationToken))
            .WithMessage(x => ValidationMessages.RequiredField(nameof(x.OwnerGuid)));
    }
}