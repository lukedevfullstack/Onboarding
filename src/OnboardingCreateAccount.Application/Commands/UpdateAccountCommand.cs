using OnboardingCreateAccount.Application.DTOs;
using MediatR;

namespace OnboardingCreateAccount.Application.Commands;

public record UpdateAccountCommand(Guid Id, string OwnerName, string Document, bool IsActive) : IRequest<AccountResponse>;
