using BankAccount.Features.Models;
using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.GetStatement;

public class GetStatementQueryHandler(IAccountService accountService)
    : IRequestHandler<GetStatementQuery, IEnumerable<Transaction>>
{
    public async Task<IEnumerable<Transaction>> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        return await accountService.GetStatement(request, cancellationToken);
    }
}