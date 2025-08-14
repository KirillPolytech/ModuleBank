using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;

namespace BankAccount.Services.Interfaces;

public interface IAccountService
{
    Task Create(Account request, CancellationToken cancellationToken);
    Task Patch(PatchAccountDto request, CancellationToken cancellationToken);
    Task Delete(DeleteAccountCommand request, CancellationToken cancellationToken);
    Task<IReadOnlyList<Account>> GetAllByOwnerId(Guid ownerId, CancellationToken cancellationToken);
    Task RegisterTransaction(RegisterTransactionCommand request, CancellationToken cancellationToken);
    Task Transfer(TransferCommand request, CancellationToken cancellationToken);
    Task<IReadOnlyList<Transaction>> GetStatement(GetStatementQuery request, CancellationToken cancellationToken);
    Task<bool> HasAccount(Guid ownerId, CancellationToken cancellationToken);
    Task<bool> HasAccount(Guid ownerId, Guid accountGuid, CancellationToken cancellationToken);

    Task<Account> GetById(Guid accountGuid, CancellationToken cancellationToken);
    Task<bool> Exists(Guid accountGuid, CancellationToken cancellationToken);

    Task AccrueInterestForAllAccountsAsync();
}