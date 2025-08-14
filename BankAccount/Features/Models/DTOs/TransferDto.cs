namespace BankAccount.Features.Models.DTOs;

/// <summary>
/// Data Transfer Object representing a funds transfer request.
/// </summary>
public class TransferDto
{
    /// <summary>
    /// The unique identifier of the account to transfer funds from.
    /// </summary>
    public required Guid From { get; set; }

    /// <summary>
    /// The unique identifier of the account to transfer funds to.
    /// </summary>
    public required Guid To { get; set; }

    /// <summary>
    /// The amount of money to transfer.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// The description of transaction.
    /// </summary>
    public required string Description { get; set; }
}