using BankAccount.Services.Interfaces;

namespace BankAccount.Services
{
    public class ClientVerification : IClientVerificationService
    {
        private readonly IAccountRepository _accountRepository;

        public ClientVerification(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> OwnerExistsAsync(Guid ownerGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_accountRepository.Owners.Any(x => x == ownerGuid));
        }
    }
}