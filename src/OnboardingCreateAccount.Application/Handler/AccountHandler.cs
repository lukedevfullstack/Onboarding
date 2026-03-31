using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Queries;
using MediatR;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Domain.Events;
using OnboardingCreateAccount.Domain.Entities;

namespace OnboardingCreateAccount.Application.Handler;

public class AccountHandler :
    IRequestHandler<CreateAccountCommand, AccountResponse>,
    IRequestHandler<UpdateAccountCommand, AccountResponse>,
    IRequestHandler<DeleteAccountCommand, bool>,
    IRequestHandler<GetAccountByIdQuery, AccountResponse?>
{
    private readonly IAccountRepository _repository;
    private readonly ICacheService _cache;
    private readonly IMediator _mediator;

    public AccountHandler(IAccountRepository repository, ICacheService cache, IMediator mediator)
    {
        _repository = repository;
        _cache = cache;
        _mediator = mediator;
    }

    public async Task<AccountResponse> Handle(CreateAccountCommand request, CancellationToken ct)
    {
        var alreadyExists = await _repository.ExistsByDocumentAsync(request.Document);
        if (alreadyExists)
            throw new Exception("Já existe uma conta cadastrada com este CPF.");

        var account = new Account(request.OwnerName, request.Document);

        await _repository.AddAsync(account);

        await _mediator.Publish(new AccountCreatedEvent(account.Id, account.OwnerName, account.Document), ct);

        return new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);
    }

    public async Task<AccountResponse?> Handle(GetAccountByIdQuery request, CancellationToken ct)
    {
        string cacheKey = $"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}";

        var cached = await _cache.GetAsync<AccountResponse>(cacheKey);
        if (cached != null) return cached;

        var account = await _repository.GetByIdAsync(request.Id);
        if (account == null) return null;

        var response = new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);

        await _cache.SetAsync(cacheKey, response, TimeSpan.FromDays(1));

        return response;
    }

    public async Task<AccountResponse> Handle(UpdateAccountCommand request, CancellationToken ct)
    {
        var account = await _repository.GetByIdAsync(request.Id);
        if (account == null) throw new Exception("Conta não encontrada");

        account.Update(request.OwnerName, request.Document, request.IsActive);
        await _repository.UpdateAsync(account);

        await _cache.RemoveAsync($"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}");

        await _mediator.Publish(new AccountUpdatedEvent(account.Id, account.Document, account.IsActive), ct);

        return new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken ct)
    {
        await _repository.DeleteAsync(request.Id);
        await _cache.RemoveAsync($"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}");
        await _mediator.Publish(new AccountDeletedEvent(request.Id), ct);
        return true;
    }
}