using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;

namespace BankAccount.Services;

public class CurrencyService : ICurrencyService
{
    public async Task<bool> IsCurrencySupported(CurrencyType currency, CancellationToken cancellationToken)
    {
        return await Task.FromResult(true);
    }
}