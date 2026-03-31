using OnboardingCreateAccount.Domain;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Infrastrsucture.Context;
using System;

namespace OnboardingCreateAccount.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    public AccountRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByDocumentAsync(string document)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Account>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Account?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Account account)
    {
        throw new NotImplementedException();
    }
    // Implementar Update, Delete e GetById...
}
