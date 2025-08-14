using System.ComponentModel.DataAnnotations;
using BankAccount.Features.Models.Enums;

namespace BankAccount.Features.Models;

/// <summary>
/// Represents a financial transaction linked to an account, including details such as transaction ID, involved accounts,
/// </summary>
public class Transaction
{
    /// <summary>
    /// Unique identifier of the transaction
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the account associated with this transaction
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Optional identifier of the counterparty's account involved in the transaction
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }

    /// <summary>
    /// Amount of money transferred in the transaction
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency type of the transaction amount
    /// </summary>
    public required CurrencyType Currency { get; set; }

    /// <summary>
    /// Type of transaction (e.g., deposit, withdrawal, transfer)
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Description or purpose of the transaction
    /// </summary>
    [MaxLength(500)] 
    public required string Description { get; set; }

    /// <summary>
    /// Date and time when the transaction occurred
    /// </summary>
    public DateTime Timestamp { get; set; }
}