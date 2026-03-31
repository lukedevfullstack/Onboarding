using OnboardingCreateAccount.Application.DTOs;
using MediatR;

namespace OnboardingCreateAccount.Application.Queries;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountResponse?>;