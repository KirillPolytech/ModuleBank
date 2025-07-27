using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;

namespace BankAccount.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Create(Account request, CancellationToken cancellationToken);
        Task<bool> Patch(PatchAccountDto request, CancellationToken cancellationToken);
        Task<bool> Delete(DeleteAccountCommand request, CancellationToken cancellationToken);
        Task<IReadOnlyList<Account>> GetAllByOwnerId(Guid ownerId, CancellationToken cancellationToken);
        Task<bool> RegisterTransaction(RegisterTransactionCommand request, CancellationToken cancellationToken);
        Task<bool> Transfer(TransferCommand request, CancellationToken cancellationToken);
        Task<IReadOnlyList<Transaction>> GetStatement(GetStatementQuery request, CancellationToken cancellationToken);
        Task<bool> HasAccount(Guid ownerId, CancellationToken cancellationToken);
        Task<bool> HasAccount(Guid ownerId, Guid accountGuid, CancellationToken cancellationToken);

        Task<Account?> GetById(Guid accountGuid, CancellationToken cancellationToken);
    }
}