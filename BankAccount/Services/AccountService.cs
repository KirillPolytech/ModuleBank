using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.ExceptionValidation;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using BankAccount.Features.Models.Enums;
using BankAccount.Persistence.Db;
using BankAccount.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace BankAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AccountService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyList<Account>> GetAllByOwnerId(Guid ownerGuid, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await context
                    .Accounts
                    .Where(x => x.OwnerId == ownerGuid)
                    .ToListAsync(cancellationToken);
        }

        public async Task<Account> GetById(Guid accountGuid, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var result = await context.Accounts.FirstAsync(x => x.Id == accountGuid, cancellationToken);
            return result;
        }

        public async Task Create(Account request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            context.Accounts.Add(request);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }
        }

        public async Task Patch(PatchAccountDto request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var account = await context.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.AccountGuid, cancellationToken);

            if (request.InterestRate != null)
                account!.InterestRate = request.InterestRate;

            if (request.Type != null)
                account!.Type = (AccountType)request.Type;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }
        }

        public async Task Delete(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var account = await context
                .Accounts
                .FirstOrDefaultAsync(x => x.Id == request.AccountGuid, cancellationToken);

            context.Accounts.Remove(account!);
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }
        }

        public async Task<IReadOnlyList<Transaction>> GetStatement(
            GetStatementQuery request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var account = await context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);

            return account!.Transactions
                .Where(t => t.Timestamp >= request.From && t.Timestamp <= request.To)
                .ToList();
        }

        public async Task RegisterTransaction(RegisterTransactionCommand request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var account = await context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.TransactionDto.AccountId, cancellationToken);

            var transactionDto = request.TransactionDto;
            account!.Transactions.Add(transactionDto.Adapt<Transaction>());

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }
        }

        public async Task Transfer(TransferCommand request, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            await using var transaction = await context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable, cancellationToken);

            var from = await context.Accounts.FirstOrDefaultAsync(x => x.Id == request.TransferDto.From, cancellationToken);
            var to = await context.Accounts.FirstOrDefaultAsync(x => x.Id == request.TransferDto.To, cancellationToken);

            from!.Balance -= request.TransferDto.Amount;
            to!.Balance += request.TransferDto.Amount;

            var registerTransactionCommandFrom = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = from.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Debit,
                    Currency = from.CurrencyType,
                    Description = request.TransferDto.Description
                }
            );


            var registerTransactionCommandTo = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = to.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Credit,
                    Currency = from.CurrencyType,
                    Description = request.TransferDto.Description
                }
            );

            await RegisterTransaction(registerTransactionCommandFrom, cancellationToken);
            await RegisterTransaction(registerTransactionCommandTo, cancellationToken);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }

            var checkFrom = await context.Accounts
                .AsNoTracking()
                .FirstAsync(a => a.Id == request.TransferDto.From, cancellationToken);

            var checkTo = await context.Accounts
                .AsNoTracking()
                .FirstAsync(a => a.Id == request.TransferDto.To, cancellationToken);

            if (checkFrom.Balance < 0 || (checkFrom.Balance + checkTo.Balance != from.Balance + to.Balance))
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new InvalidOperationException();
            }
        }

        public async Task<bool> HasAccount(Guid ownerId, Guid accountGuid, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Accounts.AnyAsync(x => x.OwnerId == ownerId, cancellationToken);
        }

        public async Task<bool> HasAccount(Guid ownerId, CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Accounts.AnyAsync(x => x.OwnerId == ownerId, cancellationToken);
        }

        public async Task AccrueInterestForAllAccountsAsync()
        {
            var context = await _contextFactory.CreateDbContextAsync();

            await foreach (var account in context.Accounts.AsAsyncEnumerable())
            {
                var accountIdParam = new NpgsqlParameter("p_account_id", account.Id);

                await context.Database.ExecuteSqlRawAsync("CALL accrue_interest(@p_account_id);", accountIdParam);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(ValidationMessages.TheDataIsOutdate);
            }
        }
    }
}