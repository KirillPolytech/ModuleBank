using BankAccount.Features.ExceptionValidation;
using FluentValidation;

namespace BankAccount.Features.Accounts.GetStatement
{
    public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
    {
        public GetStatementQueryValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage(x => ValidationMessages.RequiredField(nameof(x.AccountId)));
        }
    }
}
