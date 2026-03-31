using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Queries;
using MediatR;
using OnboardingCreateAccount.Domain;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Domain.Events;

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

    // CREATE
    public async Task<AccountResponse> Handle(CreateAccountCommand request, CancellationToken ct)
    {
        var account = new Account(request.OwnerName, request.Document);

        await _repository.AddAsync(account);

        // Notifica outras áreas (Fraude, Cartões) de forma assíncrona
        await _mediator.Publish(new AccountCreatedEvent(account.Id, account.OwnerName, account.Document), ct);

        return new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);
    }

    // READ (Otimizado para Custo AWS)
    public async Task<AccountResponse?> Handle(GetAccountByIdQuery request, CancellationToken ct)
    {
        // Chave de cache baseada no ID e no Dia Atual (yyyyMMdd)
        string cacheKey = $"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}";

        // 1. Tenta recuperar do Cache para evitar custo de consulta no DB
        var cached = await _cache.GetAsync<AccountResponse>(cacheKey);
        if (cached != null) return cached;

        // 2. Se não estiver no cache, vai ao banco de dados (Custo AWS)
        var account = await _repository.GetByIdAsync(request.Id);
        if (account == null) return null;

        var response = new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);

        // 3. Salva no Cache com expiração para o final do dia
        await _cache.SetAsync(cacheKey, response, TimeSpan.FromDays(1));

        return response;
    }

    // UPDATE
    public async Task<AccountResponse> Handle(UpdateAccountCommand request, CancellationToken ct)
    {
        var account = await _repository.GetByIdAsync(request.Id);
        if (account == null) throw new Exception("Conta não encontrada");

        account.Update(request.OwnerName, request.Document, request.IsActive);
        await _repository.UpdateAsync(account);

        // Invalida o cache para que a próxima consulta pegue o dado novo
        await _cache.RemoveAsync($"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}");

        await _mediator.Publish(new AccountUpdatedEvent(account.Id, account.Document, account.IsActive), ct);

        return new AccountResponse(account.Id, account.OwnerName, account.Document, account.IsActive);
    }

    // DELETE
    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken ct)
    {
        await _repository.DeleteAsync(request.Id);
        await _cache.RemoveAsync($"account:{request.Id}:{DateTime.UtcNow:yyyyMMdd}");
        await _mediator.Publish(new AccountDeletedEvent(request.Id), ct);
        return true;
    }
}