using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetAccount;
using BankAccount.Features.Accounts.GetAccounts;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.Patch;
using BankAccount.Features.Accounts.Update;
using BankAccount.Features.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Features.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new bank account.
        /// </summary>
        /// <param name="request">The account data to create.</param>
        /// <returns>The created account with a location URI.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand request)
        {
            var newAccount = await mediator.Send(request);
            return Created($"/accounts/{newAccount.Id}", newAccount);
        }

        /// <summary>
        /// Fully updates an existing bank account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request">The updated account data (must include the account ID).</param>
        /// <returns>The updated account, or BadRequest if the account does not exist.</returns>
        [HttpPut("/accounts/{accountId:guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountCommand request)
        {
            var result = await mediator.Send(request);
            return result ? BadRequest() : Ok(result);
        }

        /// <summary>
        /// Deletes an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to delete.</param>
        /// <returns>
        /// Returns BadRequest if deletion failed, otherwise returns Ok with the result.
        /// </returns>
        [HttpDelete("/accounts/{accountId:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var result = await mediator.Send(new DeleteAccountCommand(accountId));
            return result ? BadRequest() : Ok(result);
        }

        /// <summary>
        /// Retrieves an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to retrieve.</param>
        /// <returns>
        /// Returns BadRequest if the account is not found, otherwise returns Ok with the account data.
        /// </returns>
        [HttpGet("/accounts/{accountId:guid}")]
        public async Task<IActionResult> GetAccount([FromRoute] Guid accountId)
        {
            var account = await mediator.Send(new GetAccountQuery(accountId));
            return account is null ? BadRequest() : Ok(account);
        }

        /// <summary>
        /// Retrieves all accounts belonging to a specific owner.
        /// </summary>
        /// <param name="ownerId">The GUID of the account owner.</param>
        /// <returns>
        /// Returns BadRequest if no accounts are found, otherwise returns Ok with the list of accounts.
        /// </returns>
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] Guid ownerId)
        {
            var accounts = await mediator.Send(new GetAccountsQuery(ownerId));
            return !accounts.Any() ? BadRequest() : Ok(accounts);
        }

        /// <summary>
        /// Checks if an account exists for a given owner.
        /// </summary>
        /// <param name="ownerId">The GUID of the account owner to check.</param>
        /// <returns>
        /// Returns BadRequest if no account is found, otherwise returns Ok with the account data.
        /// </returns>
        [HttpGet("exists")]
        public async Task<IActionResult> CheckAccountExists([FromQuery] Guid ownerId)
        {
            var account = await mediator.Send(new GetAccountQuery(ownerId));
            return account is null ? BadRequest() : Ok(account);
        }

        /// <summary>
        /// Applies partial updates to an account identified by its GUID.
        /// </summary>
        /// <param name="accountId">The GUID of the account to patch.</param>
        /// <param name="request">The patch request containing updated account data.</param>
        /// <returns>
        /// Returns BadRequest if the patch operation failed, otherwise returns Ok with the result.
        /// </returns>
        [HttpPatch("{accountId}")]
        public async Task<IActionResult> PatchAccount(Guid accountId, [FromBody] PatchAccountCommand request)
        {
            var result = await mediator.Send(request with { Guid = accountId });
            return result ? BadRequest() : Ok(result);
        }

        /// <summary>
        /// Retrieves the account statement for the specified account within the given date range.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="from">The optional start date of the statement period (inclusive).</param>
        /// <param name="to">The optional end date of the statement period (inclusive).</param>
        /// <returns>
        /// Returns a list of transactions if found; otherwise, returns a <see cref="BadRequestResult"/>.
        /// </returns>
        /// <response code="200">Returns the list of transactions for the specified period.</response>
        /// <response code="400">Returned when no transactions are found for the specified criteria.</response>
        [HttpGet("{accountId:guid}/statement")]
        public async Task<IActionResult> GetAccountStatement(
            Guid accountId, 
            [FromQuery] DateTime? from, 
            [FromQuery] DateTime? to)
        {
            var transactions = await mediator.Send(new GetStatementQuery(accountId, from, to));
            return !transactions.Any() ? BadRequest() : Ok(transactions);
        }
    }
}