using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Features.Controllers;

[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new transaction for the specified account.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account for which the transaction is registered.</param>
    /// <param name="transferDto">The transaction details to register.</param>
    /// <returns>Boolean indicating whether the transaction was registered successfully.</returns>
    /// <response code="200">Transaction registered successfully.</response>
    /// <response code="400">Bad request due to invalid input data.</response>
    /// <remarks>Authorization is required.</remarks>        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost("{accountId:guid}/transactions")]
    public async Task<IActionResult> RegisterTransaction(Guid accountId, TransactionDto transferDto)
    {
        var transactions = await _mediator.Send(new RegisterTransactionCommand(transferDto));
        return Ok(transactions);
    }

    /// <summary>
    /// Executes a transfer between accounts based on the provided transfer details.
    /// </summary>
    /// <param name="transferDto">The transfer data including source, destination, and amount.</param>
    /// <returns>Operation result indicating whether the transfer was successful.</returns>
    /// <response code="200">Transfer completed successfully.</response>
    /// <response code="400">Bad request due to invalid input data.</response>
    /// <remarks>Authorization is required.</remarks>
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferDto transferDto)
    {
        await _mediator.Send(new TransferCommand(transferDto));
        return Ok(MbResult<bool>.Ok(true));
    }
}