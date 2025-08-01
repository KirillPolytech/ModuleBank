﻿using BankAccount.Features.Models.Enums;

namespace BankAccount.Features.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CounterpartyAccountId { get; set; }
        public decimal Amount { get; set; }
        public required CurrencyType Currency { get; set; }
        public TransactionType Type { get; set; }
        public required string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}