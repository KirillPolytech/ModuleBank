using BankAccount.Features.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Features.Models
{
    /// <summary>
    /// Represents a bank account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Unique identifier of the account
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identifier of the owner of this account
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Type of the account (e.g., savings, checking)
        /// </summary>
        public required AccountType Type { get; set; }

        /// <summary>
        /// Currency used by the account
        /// </summary>
        public required CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// Current balance of the account
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Optional interest rate applied to the account balance
        /// </summary>
        public decimal? InterestRate { get; set; }

        /// <summary>
        /// Date when the account was opened
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Optional date when the account was closed
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// List of transactions associated with this account
        /// </summary>
        public List<Transaction> Transactions { get; set; } = [];

        [Timestamp]
        [Column("xmin")]
        public uint Version { get; set; }
    }
}