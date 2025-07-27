namespace BankAccount.Services.Interfaces
{
    public interface IClientVerificationService
    {
        public Task<bool> OwnerExistsAsync(Guid ownerGuid, CancellationToken cancellationToken);
    }
}