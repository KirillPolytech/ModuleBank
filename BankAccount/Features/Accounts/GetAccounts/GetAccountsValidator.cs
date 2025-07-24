using BankAccount.Services.Interfaces;
using FluentValidation;

namespace BankAccount.Features.Accounts.GetAccounts
{
    public class GetAccountsValidator : AbstractValidator<GetAccountsQuery>
    {
        public GetAccountsValidator(IAccountService accountService)
        {
        }
    }
}