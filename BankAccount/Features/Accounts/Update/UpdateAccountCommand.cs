using BankAccount.Features.Models.DTOs;
using MediatR;

namespace BankAccount.Features.Accounts.Update;

public record UpdateAccountCommand(AccountDto AccountDto) : IRequest<Unit>;