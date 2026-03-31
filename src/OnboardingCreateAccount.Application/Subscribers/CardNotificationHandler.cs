using MediatR;
using OnboardingCreateAccount.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingCreateAccount.Application.Subscribers;


public class CardNotificationHandler : INotificationHandler<AccountCreatedEvent>
{
    public Task Handle(AccountCreatedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[CARTÕES] Gerando oferta de crédito para: {notification.Name}");
        return Task.CompletedTask;
    }
}
