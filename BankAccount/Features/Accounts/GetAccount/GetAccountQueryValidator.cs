using BankAccount.Features.ExceptionValidation;
using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.GetAccount
{
    public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
    {
        public GetAccountQueryValidator(IAccountService accountService)
        {
            RuleFor(x => x.AccountGuid)
                .NotEmpty()
                .MustAsync(async (accountGuid, cancellationToken)
                    => (await accountService.GetById(accountGuid, cancellationToken)) != null)
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.AccountGuid)));
        }
    }
}