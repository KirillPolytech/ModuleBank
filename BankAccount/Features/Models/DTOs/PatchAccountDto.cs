using BankAccount.Features.Models.Enums;
using System.ComponentModel;

namespace BankAccount.Features.Models.DTOs
{
    public class PatchAccountDto
    {
        public Guid? OwnerId { get; set; }
        public AccountType? Type { get; set; }
        public CurrencyType? Currency { get; set; }
        public decimal? Balance { get; set; }
        public decimal? InterestRate { get; set; }
    }
}