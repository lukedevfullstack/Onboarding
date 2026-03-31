using MediatR;
using OnboardingCreateAccount.Application.DTOs;

namespace OnboardingCreateAccount.Application.Commands;

public record CreateAccountCommand : IRequest<AccountResponse>
{
    public string OwnerName { get; init; } = string.Empty;
    public string Document { get; init; } = string.Empty;
}