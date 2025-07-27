using BankAccount.Features.Models.Enums;

namespace BankAccount.Services.Interfaces
{
    public interface ICurrencyService
    {
        public Task<bool> IsCurrencySupported(CurrencyType currency, CancellationToken cancellationToken);
    }
}