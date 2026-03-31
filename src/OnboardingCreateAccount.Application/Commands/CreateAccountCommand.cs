using OnboardingCreateAccount.Application.DTOs;
using MediatR;

namespace OnboardingCreateAccount.Application.Commands;

public record CreateAccountCommand(string OwnerName, string Document) : IRequest<AccountResponse>;
