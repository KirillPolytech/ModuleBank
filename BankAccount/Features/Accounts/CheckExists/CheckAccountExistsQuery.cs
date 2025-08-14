using MediatR;

namespace BankAccount.Features.Accounts.CheckExists;

public record CheckAccountExistsQuery(Guid AccountGuid) : IRequest<bool>;