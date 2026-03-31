using MediatR;

namespace OnboardingCreateAccount.Application.Commands;

public record DeleteAccountCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
