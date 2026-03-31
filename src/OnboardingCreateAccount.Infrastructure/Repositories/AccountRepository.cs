using Microsoft.EntityFrameworkCore;
using OnboardingCreateAccount.Domain.Entities;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Infrastrsucture.Context;

namespace OnboardingCreateAccount.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Accounts
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsByDocumentAsync(string document)
    {
        return await _context.Accounts
            .AnyAsync(a => a.Document == document);
    }
}