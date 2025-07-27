namespace BankAccount.Features.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object representing a funds transfer request.
    /// </summary>
    public class TransferDto
    {
        /// <summary>
        /// The unique identifier of the account to transfer funds from.
        /// </summary>
        public Guid From { get; set; }

        /// <summary>
        /// The unique identifier of the account to transfer funds to.
        /// </summary>
        public Guid To { get; set; }

        /// <summary>
        /// The amount of money to transfer.
        /// </summary>
        public decimal Amount { get; set; }
    }
}