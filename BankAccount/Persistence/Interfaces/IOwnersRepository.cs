namespace BankAccount.Persistence.Interfaces
{
    public interface IOwnersRepository
    {
        public List<Guid> Owners { get; set; }
    }
}