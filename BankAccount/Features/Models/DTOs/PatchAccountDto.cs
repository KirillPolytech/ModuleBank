using BankAccount.Features.Models.Enums;

namespace BankAccount.Features.Models.DTOs;

/// <summary>
/// Data Transfer Object for partially updating account information.
/// </summary>
public class PatchAccountDto
{
    /// <summary>
    /// Unique identifier of the account to be updated.
    /// </summary>
    public Guid AccountGuid { get; set; }

    /// <summary>
    /// Optional new type of the account.
    /// </summary>
    public AccountType? Type { get; set; }

    /// <summary>
    /// Optional new currency type of the account.
    /// </summary>
    public CurrencyType? Currency { get; set; }

    /// <summary>
    /// Optional new interest rate for the account.
    /// </summary>
    public decimal? InterestRate { get; set; }
}