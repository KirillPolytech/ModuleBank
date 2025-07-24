using BankAccount.Features.Models.Enums;

namespace BankAccount.Features.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public required AccountType Type { get; set; }
        public required CurrencyType CurrencyType { get; set; }
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }
}