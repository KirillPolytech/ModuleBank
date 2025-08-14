using BankAccount.Features.Models;
using BankAccount.Persistence.Db;

namespace BankAccount.UnitTests;

public class TestService
{
    private readonly AppDbContext _dbContext;

    public TestService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<decimal> GetTotalBalanceAsync()
    {
        var accounts = _dbContext.Set<Account>();
        return Task.FromResult(accounts.Sum(a => a.Balance));
    }
}