using FluentValidation;

namespace BankAccount.Features.Accounts.GetStatement
{
    public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
    {
        public GetStatementQueryValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId must exist");
        }
    }
}
