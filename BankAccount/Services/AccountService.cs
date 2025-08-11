using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using BankAccount.Features.Models.Enums;
using BankAccount.Persistence.Db;
using BankAccount.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbContext;

        public AccountService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Account>> GetAllByOwnerId(Guid ownerGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                _dbContext
                    .Accounts
                    .Where(x => x.OwnerId == ownerGuid)
                    .ToList());
        }

        public async Task<Account?> GetById(Guid accountGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbContext.Accounts.FirstOrDefault(x => x.Id == accountGuid));
        }

        public async Task Create(Account request, CancellationToken cancellationToken)
        {
            _dbContext.Accounts.Add(request);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Patch(PatchAccountDto request, CancellationToken cancellationToken)
        {
            foreach (var account in _dbContext.Accounts)
            {
                if (account.Id != request.AccountGuid)
                    continue;

                if (request.InterestRate != null)
                    account.InterestRate = request.InterestRate;

                if (request.Type != null)
                    account.Type = (AccountType)request.Type;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task Delete(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == request.AccountGuid);

            _dbContext.Accounts.Remove(account.Result!);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Transaction>> GetStatement(
            GetStatementQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                _dbContext
                    .Accounts
                    .Include(account => account.Transactions)
                    .First(x => x.OwnerId == request.AccountId)
                    .Transactions
                    .Where(t => t.Timestamp >= request.From && t.Timestamp <= request.To)
                    .ToList());
        }

        public async Task RegisterTransaction(RegisterTransactionCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.TransactionDto.AccountId, cancellationToken);
            
            var transactionDto = request.TransactionDto;
            account!.Transactions.Add(transactionDto.Adapt<Transaction>());

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Transfer(TransferCommand request, CancellationToken cancellationToken)
        {
            var from = _dbContext.Accounts.First(x => x.Id == request.TransferDto.From);
            var to = _dbContext.Accounts.First(x => x.Id == request.TransferDto.To);

            from.Balance -= request.TransferDto.Amount;
            to.Balance += request.TransferDto.Amount;

            var registerTransactionCommandFrom = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = from.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Debit,
                    Currency = from.CurrencyType
                }
            );


            var registerTransactionCommandTo = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = from.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Credit,
                    Currency = from.CurrencyType
                }
            );

            await Task
                .WhenAll(
                   RegisterTransaction(registerTransactionCommandFrom, cancellationToken),
                RegisterTransaction(registerTransactionCommandTo, cancellationToken));

        }

        public async Task<bool> HasAccount(Guid ownerId, Guid accountGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbContext.Accounts.Any(x => x.OwnerId == ownerId));
        }

        public async Task<bool> HasAccount(Guid ownerId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_dbContext.Accounts.Any(x => x.OwnerId == ownerId));
        }
    }
}