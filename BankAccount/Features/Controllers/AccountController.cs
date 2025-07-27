using BankAccount.Features.Accounts.CheckExists;
using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetAccount;
using BankAccount.Features.Accounts.GetAccounts;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.Patch;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Accounts.Update;
using BankAccount.Features.Models;
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
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand request)
        {
            var newAccount = await mediator.Send(request);
            return Created($"{newAccount.Id}", newAccount);
        }

        /// <summary>
        /// Fully updates an existing bank account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request">The updated account data (must include the account ID).</param>
        /// <returns>The updated account, or BadRequest if the account does not exist.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountCommand request)
        {
            var result = await mediator.Send(request);
            return !result ? BadRequest() : Ok(result);
        }

        /// <summary>
        /// Deletes an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to delete.</param>
        /// <returns>
        /// Returns BadRequest if deletion failed, otherwise returns Ok with the result.
        /// </returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var result = await mediator.Send(new DeleteAccountCommand(accountId));
            return !result ? BadRequest() : Ok(result);
        }

        /// <summary>
        /// Retrieves an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to retrieve.</param>
        /// <returns>
        /// Returns BadRequest if the account is not found, otherwise returns Ok with the account data.
        /// </returns>
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{accountId:guid}")]
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
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] Guid ownerId)
        {
            var accounts = await mediator.Send(new GetAccountsQuery(ownerId));
            return !accounts.Any() ? BadRequest() : Ok(accounts);
        }

        /// <summary>
        /// Checks whether an account with the specified GUID exists.
        /// </summary>
        /// <param name="accountGuid">The unique identifier of the account to check.</param>
        /// <returns>
        /// Returns <c>200 OK</c> with <c>true</c> if the account exists;
        /// returns <c>400 Bad Request</c> if the account does not exist or the request is invalid.
        /// </returns>
        /// <response code="200">Account exists</response>
        /// <response code="400">Account does not exist or the request is invalid</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{accountGuid:guid}/exists")]
        public async Task<IActionResult> CheckAccountExists(Guid accountGuid)
        {
            var account = await mediator.Send(new CheckAccountExistsQuery(accountGuid));
            return !account ? BadRequest() : Ok(account);
        }

        /// <summary>
        /// Applies partial updates to an account identified by its GUID.
        /// </summary>
        /// <param name="accountId">The GUID of the account to patch.</param>
        /// <param name="request">The patch request containing updated account data.</param>
        /// <returns>
        /// Returns BadRequest if the patch operation failed, otherwise returns Ok with the result.
        /// </returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{accountId:guid}")]
        public async Task<IActionResult> PatchAccount(Guid accountId, [FromBody] PatchAccountCommand request)
        {
            var result = await mediator.Send(request with { AccountId = accountId });
            return !result ? BadRequest() : Ok(result);
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
        [ProducesResponseType(typeof(IEnumerable<Transaction?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{accountId:guid}/statement")]
        public async Task<IActionResult> GetAccountStatement(
            Guid accountId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var transactions = await mediator.Send(new GetStatementQuery(accountId, from, to));
            return !transactions.Any() ? BadRequest() : Ok(transactions);
        }

        /// <summary>
        /// Performs a funds transfer between accounts.
        /// </summary>
        /// <param name="transferDto">Transfer details including amount, source, and destination accounts.</param>
        /// <returns>Returns HTTP 200 OK with a boolean indicating success, or HTTP 400 Bad Request if the transfer fails.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto transferDto)
        {
            var transactions = await mediator.Send(new TransferCommand(transferDto));
            return !transactions ? BadRequest() : Ok(transactions);
        }

        /// <summary>
        /// Registers a transaction for the specified account.
        /// </summary>
        /// <param name="accountId">The ID of the account for which the transaction is registered.</param>
        /// <param name="transferDto">The transaction details to register.</param>
        /// <returns>Returns HTTP 200 OK with a boolean indicating success, or HTTP 400 Bad Request if the registration fails.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{accountId:guid}/transactions")]
        public async Task<IActionResult> RegisterTransaction(Guid accountId, TransactionDto transferDto)
        {
            var transactions = await mediator.Send(new RegisterTransactionCommand(transferDto));
            return !transactions ? BadRequest() : Ok(transactions);
        }
    }
}