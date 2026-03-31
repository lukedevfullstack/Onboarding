using MediatR;

namespace OnboardingCreateAccount.Domain.Events;

public record AccountCreatedEvent(Guid Id, string Name, string Document) : INotification;

public record AccountUpdatedEvent(Guid Id, string Document, bool IsActive) : INotification;

public record AccountDeletedEvent(Guid Id) : INotification;

