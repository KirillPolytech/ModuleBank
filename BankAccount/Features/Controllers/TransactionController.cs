using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Features.Controllers
{
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a transaction for the specified account.
        /// </summary>
        /// <param name="accountId">The ID of the account for which the transaction is registered.</param>
        /// <param name="transferDto">The transaction details to register.</param>
        /// <returns>Returns HTTP 200 OK with a boolean indicating success, or HTTP 400 Bad Request if the registration fails.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountId:guid}/transactions")]
        public async Task<IActionResult> RegisterTransaction(Guid accountId, TransactionDto transferDto)
        {
            var transactions = await _mediator.Send(new RegisterTransactionCommand(transferDto));
            return Ok(transactions);
        }
    }
}