using BankAccount.Features.Models;
using BankAccount.Features.Models.Enums;
using BankAccount.Persistence.Interfaces;

namespace BankAccount.Persistence.InMemory
{
    public class InMemoryAccountsRepository : IAccountRepository
    {
        public List<Account> Accounts { get; set; }

        public InMemoryAccountsRepository()
        {
            Accounts =
            [
                new Account
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                    OwnerId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                    Balance = 1000,
                    CurrencyType = CurrencyType.Rub,
                    Type = AccountType.Credit,
                    Transactions =
                    [
                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                            Amount = 2500,
                            Currency = CurrencyType.Rub,
                            Type = TransactionType.Credit,
                            Description = "Зачисление зарплаты",
                            Timestamp = DateTime.UtcNow.AddDays(-5)
                        },

                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                            Amount = -700,
                            Currency = CurrencyType.Rub,
                            Type = TransactionType.Debit,
                            Description = "Оплата ЖКХ",
                            Timestamp = DateTime.UtcNow.AddDays(-4)
                        },

                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                            Amount = -1200,
                            Currency = CurrencyType.Rub,
                            Type = TransactionType.Debit,
                            Description = "Покупка в магазине",
                            Timestamp = DateTime.UtcNow.AddDays(-2)
                        },

                        new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                            Amount = 1000,
                            Currency = CurrencyType.Rub,
                            Type = TransactionType.Credit,
                            Description = "Перевод от друга",
                            Timestamp = DateTime.UtcNow.AddDays(-1)
                        }
                    ]
                }

            ];
        }
    }
}