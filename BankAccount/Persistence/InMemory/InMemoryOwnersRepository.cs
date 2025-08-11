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
                Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ];                  
        }
    }
}