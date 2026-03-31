using MediatR;
using OnboardingCreateAccount.Application.DTOs;

namespace OnboardingCreateAccount.Application.Commands;

public record UpdateAccountCommand : IRequest<AccountResponse>
{
    public Guid Id { get; set; }
    public string OwnerName { get; init; } = string.Empty;
    public string Document { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}