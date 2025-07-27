using BankAccount.Features.Models;

namespace BankAccount.Services.Interfaces
{
    public interface IAccountRepository
    {
        public List<Account> Accounts { get; set; }
        public List<Guid> Owners { get; set; }
    }
}