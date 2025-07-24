using BankAccount.Services.Interfaces;
using MediatR;

namespace BankAccount.Features.Accounts.Delete
{
    public class DeleteAccountCommandHandler(IAccountService accountService)
        : IRequestHandler<DeleteAccountCommand, bool>
    {
        public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            return await accountService.Delete(request, cancellationToken);
        }
    }
}
