using MediatR;
using OnboardingCreateAccount.Domain.Events;

namespace OnboardingCreateAccount.Application.Subscribers;

public class FraudNotificationHandler : INotificationHandler<AccountCreatedEvent>
{
    public Task Handle(AccountCreatedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[FRAUDE] Monitorando nova conta do CPF: {notification.Document}");
        return Task.CompletedTask;
    }
}
