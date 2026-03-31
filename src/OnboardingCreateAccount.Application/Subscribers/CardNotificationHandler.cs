using MediatR;
using OnboardingCreateAccount.Domain.Events;

namespace OnboardingCreateAccount.Application.Subscribers;

public class CardNotificationHandler : INotificationHandler<AccountCreatedEvent>
{
    public Task Handle(AccountCreatedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[CARTÕES] Gerando oferta de crédito para: {notification.Name}");
        return Task.CompletedTask;
    }
}
