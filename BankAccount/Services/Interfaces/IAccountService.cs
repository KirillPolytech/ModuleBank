using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Models;

namespace BankAccount.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account?>> GetAllByOwnerId(Guid ownerId, CancellationToken cancellationToken);
        Task<Account?> GetById(Guid guid, CancellationToken cancellationToken);
        Task<bool> HasAccount(Guid ownerId);
        Task<bool> CreateAsync(Account request, CancellationToken cancellationToken);
        Task<bool> Update(Account request, CancellationToken cancellationToken);
        Task<bool> Delete(DeleteAccountCommand request, CancellationToken cancellationToken);
        Task<IEnumerable<Transaction?>> GetStatement(GetStatementQuery request, CancellationToken cancellationToken);
        Task<bool> OwnerExistsAsync(Guid guid);
        Task<bool> OwnerExistsAsync(Guid? guid);
        Task<bool> IsCurrencySupportedAsync(string currency);

        //Task<bool> Patch(Guid id, PatchAccountRequest request);
        //Task<bool> RegisterTransaction(RegisterTransactionRequest request);
        //Task<bool> Transfer(TransferRequest request);
    }
}