using BankAccount.Features.Models;

namespace BankAccount.Persistence.Interfaces;

public interface IAccountRepository
{
    public List<Account> Accounts { get; set; }
}