using BankAccount.Persistence.Interfaces;

namespace BankAccount.Persistence.InMemory
{
    public class InMemoryOwnersRepository : IOwnersRepository
    {
        public List<Guid> Owners { get; set; }

        public InMemoryOwnersRepository()
        {
            Owners =
            [
                Guid.Parse("10000000-0000-0000-0000-000000000000")
            ];                  
        }
    }
}