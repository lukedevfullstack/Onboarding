using MediatR;

namespace OnboardingCreateAccount.Application.Commands;

public record DeleteAccountCommand(Guid Id) : IRequest<bool>;
