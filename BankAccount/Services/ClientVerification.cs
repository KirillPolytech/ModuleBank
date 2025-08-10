using BankAccount.Persistence.Interfaces;
using BankAccount.Services.Interfaces;

namespace BankAccount.Services
{
    public class ClientVerification : IClientVerificationService
    {
        private readonly IOwnersRepository _repository;

        public ClientVerification(IOwnersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> OwnerExistsAsync(Guid ownerGuid, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_repository.Owners.Any(x => x == ownerGuid));
        }
    }
}