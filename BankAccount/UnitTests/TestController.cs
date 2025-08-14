using Microsoft.AspNetCore.Mvc;

namespace BankAccount.UnitTests;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly TestService _accountService;

    public TestController(TestService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("totalbalance")]
    public async Task<ActionResult<decimal>> GetTotalBalance()
    {
        var totalBalance = await _accountService.GetTotalBalanceAsync();
        return Ok(totalBalance);
    }
}