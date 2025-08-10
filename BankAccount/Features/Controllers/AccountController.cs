using BankAccount.Features.Accounts.CheckExists;
using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetAccount;
using BankAccount.Features.Accounts.GetAccounts;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.Patch;
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
        /// Creates a new account based on the provided request data.
        /// </summary>
        /// <param name="request">The command containing the necessary information to create the account.</param>
        /// <returns>Operation result containing the created account data.</returns>
        /// <response code="201">Account successfully created.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(ActionResult<MbResult<AccountDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand request)
        {
            var newAccount = await mediator.Send(request);
            return Ok(MbResult<AccountDto>.Ok(newAccount));
        }

        /// <summary>
        /// Updates an existing account identified by the specified account ID.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account to update.</param>
        /// <param name="request">The command containing updated account data.</param>
        /// <returns>Operation result indicating whether the update was successful.</returns>
        /// <response code="200">Account updated successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(MbResult<bool>.Ok(true));
        }

        /// <summary>
        /// Deletes an existing account identified by the specified account ID.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account to delete.</param>
        /// <returns>Operation result indicating whether the deletion was successful.</returns>
        /// <response code="200">Account deleted successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var result = await mediator.Send(new DeleteAccountCommand(accountId));
            return Ok(MbResult<bool>.Ok(true));
        }

        /// <summary>
        /// Retrieves the details of an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account to retrieve.</param>
        /// <returns>Operation result containing the account details.</returns>
        /// <response code="200">Account retrieved successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccount([FromRoute] Guid accountId)
        {
            var account = await mediator.Send(new GetAccountQuery(accountId));
            return Ok(MbResult<AccountDto>.Ok(account!));
        }

        /// <summary>
        /// Retrieves a list of accounts belonging to the specified owner.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the owner whose accounts are requested.</param>
        /// <returns>Operation result containing a list of account details.</returns>
        /// <response code="200">Accounts retrieved successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<List<AccountDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("getAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] Guid ownerId)
        {
            var accounts = await mediator.Send(new GetAccountsQuery(ownerId));
            return Ok(MbResult<List<AccountDto>>.Ok(accounts));
        }

        /// <summary>
        /// Checks whether an account with the specified unique identifier exists.
        /// </summary>
        /// <param name="accountGuid">The unique identifier of the account to check.</param>
        /// <returns>Operation result indicating if the account exists (true) or not (false).</returns>
        /// <response code="200">Account existence check completed successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountGuid:guid}/exists")]
        public async Task<IActionResult> CheckAccountExists(Guid accountGuid)
        {
            var account = await mediator.Send(new CheckAccountExistsQuery(accountGuid));
            return Ok(MbResult<bool>.Ok(account));
        }

        /// <summary>
        /// Partially updates an existing account identified by the specified account ID.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account to update.</param>
        /// <param name="request">The command containing the partial update data.</param>
        /// <returns>Operation result indicating whether the update was successful.</returns>
        /// <response code="200">Account updated successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPatch("{accountId:guid}")]
        public async Task<IActionResult> PatchAccount(Guid accountId, [FromBody] PatchAccountCommand request)
        {
            var result = await mediator.Send(request with { AccountId = accountId });
            return Ok(MbResult<bool>.Ok(true));
        }

        /// <summary>
        /// Retrieves the account statement (list of transactions) for a specified account within a date range.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="from">Start date of the statement period.</param>
        /// <param name="to">End date of the statement period.</param>
        /// <returns>Operation result containing a list of transactions.</returns>
        /// <response code="200">Account statement retrieved successfully.</response>
        /// <response code="400">Bad request due to invalid input data.</response>
        /// <remarks>Authorization is required.</remarks>
        [ProducesResponseType(typeof(MbResult<List<Transaction?>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{accountId:guid}/statement")]
        public async Task<IActionResult> GetAccountStatement(
            Guid accountId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var transactions = await mediator.Send(new GetStatementQuery(accountId, from, to));
            return Ok(MbResult<List<Transaction?>>.Ok(transactions!));
        }
    }
}