using BankAccount.Features.Models.Enums;

namespace BankAccount.Features.Models.DTOs;

/// <summary>
/// Data Transfer Object representing a financial transaction.
/// </summary>
public class TransactionDto
{
    /// <summary>
    /// The unique identifier of the account associated with the transaction.
    /// </summary>
    public required Guid AccountId { get; set; }

    /// <summary>
    /// Optional identifier of the counterparty's account involved in the transaction
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }


    /// <summary>
    /// The amount of the transaction.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// The currency type of the transaction amount.
    /// </summary>
    public required CurrencyType Currency { get; set; }

    /// <summary>
    /// The type of the transaction (e.g., debit, credit).
    /// </summary>
    public required TransactionType Type { get; set; }

    /// <summary>
    /// Optional description or note about the transaction.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date and time when the transaction occurred.
    /// </summary>
    public required DateTime Timestamp { get; set; }
}