using MediatR;
using OnboardingCreateAccount.Domain.Events;

namespace OnboardingCreateAccount.Application.Subscribers;

public class AccountSyncHandler :
    INotificationHandler<AccountUpdatedEvent>,
    INotificationHandler<AccountDeletedEvent>
{
    public Task Handle(AccountUpdatedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[SYNC] Atualizando status da conta {notification.Id} em todos os sistemas satélites.");
        return Task.CompletedTask;
    }

    public Task Handle(AccountDeletedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[SYNC] Solicitando revogação de acessos e cancelamento de cartões para ID: {notification.Id}.");
        return Task.CompletedTask;
    }
}