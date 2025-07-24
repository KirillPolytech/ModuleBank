using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Models;
using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;

namespace BankAccount.Services
{
    public class InMemoryAccountService : IAccountService
    {
        private readonly List<Account> _accounts = [];
        private readonly HashSet<Guid> _owners = [];

        public InMemoryAccountService()
        {
            // Initialize with some default accounts.
            _accounts.Add(new Account
            {
                Id = Guid.NewGuid(),
                Balance = 1000,
                CurrencyType = CurrencyType.Rub,
                Type = AccountType.Credit,
                Transactions =
                [
                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        Amount = 2500,
                        Currency = CurrencyType.Rub,
                        Type = TransactionType.Credit,
                        Description = "Зачисление зарплаты",
                        Timestamp = DateTime.UtcNow.AddDays(-5)
                    },

                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        Amount = -700,
                        Currency = CurrencyType.Rub,
                        Type = TransactionType.Debit,
                        Description = "Оплата ЖКХ",
                        Timestamp = DateTime.UtcNow.AddDays(-4)
                    },

                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        Amount = -1200,
                        Currency = CurrencyType.Rub,
                        Type = TransactionType.Debit,
                        Description = "Покупка в магазине",
                        Timestamp = DateTime.UtcNow.AddDays(-2)
                    },

                    new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        Amount = 1000,
                        Currency = CurrencyType.Rub,
                        Type = TransactionType.Credit,
                        Description = "Перевод от друга",
                        Timestamp = DateTime.UtcNow.AddDays(-1)
                    }
                ]
            });
            _owners.Add(_accounts[0].OwnerId);
        }

        public Task<IEnumerable<Account?>> GetAllByOwnerId(Guid ownerGuid, CancellationToken cancellationToken)
        {
            return Task.FromResult(_accounts.Where(x => x.OwnerId == ownerGuid).Cast<Account?>()); ;
        }

        public Task<Account?> GetById(Guid guid, CancellationToken cancellationToken)
        {
            return Task.FromResult(_accounts.FirstOrDefault(x => x.Id == guid));
        }

        public Task<bool> HasAccount(Guid ownerId)
        {
            return Task.FromResult(_accounts.FirstOrDefault(x => x.OwnerId == ownerId) != null);
        }

        public Task<bool> CreateAsync(Account request, CancellationToken cancellationToken)
        {
            _accounts.Add(request);
            return Task.FromResult(true);
        }

        public Task<bool> Update(Account request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction?>> GetStatement(
            GetStatementQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _accounts
                .First(x => x.OwnerId == request.AccountId)
                .Transactions
                .Where(t => t.Timestamp >= request.From && t.Timestamp <= request.To)
                .AsEnumerable()
                .Cast<Transaction?>());
        }

        public Task<bool> OwnerExistsAsync(Guid guid)
        {
            return Task.FromResult(_owners.Any(x => x == guid));
        }

        public Task<bool> OwnerExistsAsync(Guid? guid)
        {
            return Task.FromResult(_owners.FirstOrDefault(x => x == guid) == Guid.Empty);
        }

        public Task<bool> IsCurrencySupportedAsync(string currency)
        {
            //var exists = CurrencyType. .Any(x => x == currency);
            return Task.FromResult(true);
        }
    }
}