using BankAccount.Features.Models.Enums;
using System.ComponentModel;

namespace BankAccount.Features.Models.DTOs;

/// <summary>
/// DTO representing a bank account.
/// </summary>
public class AccountDto
{
    /// <summary>
    /// Unique identifier of the account.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the account owner. Can be null.
    /// </summary>
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Type of the account (e.g., savings, deposit).
    /// </summary>
    [DefaultValue(1)]
    public required AccountType Type { get; set; }

    /// <summary>
    /// Currency of the account.
    /// </summary>
    public required CurrencyType Currency { get; set; }

    /// <summary>
    /// Current balance of the account.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Interest rate applied to the account, if applicable. Can be null.
    /// </summary>
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Date when the account was opened.
    /// </summary>
    public DateTime OpenDate { get; set; }
}