using BankAccount.Features.Accounts.Delete;
using BankAccount.Features.Accounts.GetStatement;
using BankAccount.Features.Accounts.RegisterTransaction;
using BankAccount.Features.Accounts.Transfer;
using BankAccount.Features.Models;
using BankAccount.Features.Models.DTOs;
using BankAccount.Features.Models.Enums;
using BankAccount.Services.Interfaces;
using Mapster;

namespace BankAccount.Services
{
    public class InMemoryAccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public InMemoryAccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IReadOnlyList<Account>> GetAllByOwnerId(Guid ownerGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                _accountRepository
                    .Accounts
                    .Where(x => x.OwnerId == ownerGuid)
                    .ToList());
        }

        public async Task<Account?> GetById(Guid accountGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_accountRepository.Accounts.FirstOrDefault(x => x.Id == accountGuid));
        }

        public async Task<bool> Create(Account request, CancellationToken cancellationToken)
        {
            _accountRepository.Accounts.Add(request);
            return await Task.FromResult(true);
        }

        public async Task<bool> Patch(PatchAccountDto request, CancellationToken cancellationToken)
        {
            for (var i = 0; i < _accountRepository.Accounts.Count; i++)
            {
                if (_accountRepository.Accounts.ElementAt(i).Id != request.AccountGuid)
                    continue;

                if (request.InterestRate != null)
                    _accountRepository.Accounts[i].InterestRate = request.InterestRate;

                if (request.Type != null)
                    _accountRepository.Accounts[i].Type = (AccountType)request.Type;
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> Delete(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _accountRepository.Accounts.FirstOrDefault(x => x.Id == request.AccountGuid);
            if (account == null)
                return await Task.FromResult(false);

            _accountRepository.Accounts.Remove(account);
            return await Task.FromResult(true);
        }

        public async Task<IReadOnlyList<Transaction>> GetStatement(
            GetStatementQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                _accountRepository.Accounts
                .First(x => x.OwnerId == request.AccountId)
                .Transactions
                .Where(t => t.Timestamp >= request.From && t.Timestamp <= request.To)
                .ToList());
        }

        public async Task<bool> RegisterTransaction(RegisterTransactionCommand request, CancellationToken cancellationToken)
        {
            var account = _accountRepository.Accounts.First(x => x.Id == request.TransactionDto.AccountId);
            var transactionDto = request.TransactionDto;
            account.Transactions.Add(transactionDto.Adapt<Transaction>());
            return await Task.FromResult(true);
        }

        public async Task<bool> Transfer(TransferCommand request, CancellationToken cancellationToken)
        {
            var from = _accountRepository.Accounts.First(x => x.Id == request.TransferDto.From);
            var to = _accountRepository.Accounts.First(x => x.Id == request.TransferDto.To);

            from.Balance -= request.TransferDto.Amount;
            to.Balance += request.TransferDto.Amount;

            var registerTransactionCommandFrom = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = from.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Debit,
                    Currency = from.CurrencyType
                }
            );


            var registerTransactionCommandTo = new RegisterTransactionCommand
            (
                new TransactionDto
                {
                    Amount = request.TransferDto.Amount,
                    AccountId = from.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Credit,
                    Currency = from.CurrencyType
                }
            );

            await Task
                .WhenAll(
                   RegisterTransaction(registerTransactionCommandFrom, cancellationToken),
                RegisterTransaction(registerTransactionCommandTo, cancellationToken));

            return await Task.FromResult(true);
        }

        public async Task<bool> HasAccount(Guid ownerId, Guid accountGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_accountRepository.Accounts.Any(x => x.OwnerId == ownerId));
        }

        public async Task<bool> HasAccount(Guid ownerId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_accountRepository.Accounts.Any(x => x.OwnerId == ownerId));
        }
    }
}