using BankAccount.Features.Accounts.CheckExists;
using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetAccount;
using BankAccount.Features.Accounts.GetAccounts;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.Patch;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Accounts.Update;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(ActionResult<MbResult<AccountDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("create")]
        public async Task<MbResult<AccountDto>> CreateAccount([FromBody] CreateAccountCommand request)
        {
            var newAccount = await mediator.Send(request);
            return MbResult<AccountDto>.Ok(newAccount);
        }

        /// <summary>
        /// Fully updates an existing bank account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request">The updated account data (must include the account ID).</param>
        /// <returns>The updated account, or BadRequest if the account does not exist.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPut("{accountId:guid}")]
        public async Task<ActionResult<MbResult<bool>>> UpdateAccount(Guid accountId, [FromBody] UpdateAccountCommand request)
        {
            var result = await mediator.Send(request);
            return MbResult<bool>.Ok(result);
        }

        /// <summary>
        /// Deletes an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to delete.</param>
        /// <returns>
        /// Returns BadRequest if deletion failed, otherwise returns Ok with the result.
        /// </returns>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpDelete("{accountId:guid}")]
        public async Task<MbResult<bool>> DeleteAccount(Guid accountId)
        {
            var result = await mediator.Send(new DeleteAccountCommand(accountId));
            return MbResult<bool>.Ok(result);
        }

        /// <summary>
        /// Retrieves an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The GUID of the account to retrieve.</param>
        /// <returns>
        /// Returns BadRequest if the account is not found, otherwise returns Ok with the account data.
        /// </returns>
        [ProducesResponseType(typeof(MbResult<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountId:guid}")]
        public async Task<MbResult<AccountDto>> GetAccount([FromRoute] Guid accountId)
        {
            var account = await mediator.Send(new GetAccountQuery(accountId));
            return MbResult<AccountDto>.Ok(account!);
        }

        /// <summary>
        /// Retrieves all accounts belonging to a specific owner.
        /// </summary>
        /// <param name="ownerId">The GUID of the account owner.</param>
        /// <returns>
        /// Returns BadRequest if no accounts are found, otherwise returns Ok with the list of accounts.
        /// </returns>
        [ProducesResponseType(typeof(MbResult<List<AccountDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("getAccounts")]
        public async Task<MbResult<List<AccountDto>>> GetAccounts([FromQuery] Guid ownerId)
        {
            var accounts = await mediator.Send(new GetAccountsQuery(ownerId));
            return MbResult<List<AccountDto>>.Ok(accounts);
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
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountGuid:guid}/exists")]
        public async Task<MbResult<bool>> CheckAccountExists(Guid accountGuid)
        {
            var account = await mediator.Send(new CheckAccountExistsQuery(accountGuid));
            return MbResult<bool>.Ok(account);
        }

        /// <summary>
        /// Applies partial updates to an account identified by its GUID.
        /// </summary>
        /// <param name="accountId">The GUID of the account to patch.</param>
        /// <param name="request">The patch request containing updated account data.</param>
        /// <returns>
        /// Returns BadRequest if the patch operation failed, otherwise returns Ok with the result.
        /// </returns>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPatch("{accountId:guid}")]
        public async Task<MbResult<bool>> PatchAccount(Guid accountId, [FromBody] PatchAccountCommand request)
        {
            var result = await mediator.Send(request with { AccountId = accountId });
            return MbResult<bool>.Ok(result);
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
        [ProducesResponseType(typeof(MbResult<List<Transaction?>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountId:guid}/statement")]
        public async Task<MbResult<List<Transaction?>>> GetAccountStatement(
            Guid accountId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var transactions = await mediator.Send(new GetStatementQuery(accountId, from, to));
            return MbResult<List<Transaction?>>.Ok(transactions!);
        }

        /// <summary>
        /// Performs a funds transfer between accounts.
        /// </summary>
        /// <param name="transferDto">Transfer details including amount, source, and destination accounts.</param>
        /// <returns>Returns HTTP 200 OK with a boolean indicating success, or HTTP 400 Bad Request if the transfer fails.</returns>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("transfer")]
        public async Task<MbResult<bool>> Transfer([FromBody] TransferDto transferDto)
        {
            var result = await mediator.Send(new TransferCommand(transferDto));
            return MbResult<bool>.Ok(result);
        }
    }
}