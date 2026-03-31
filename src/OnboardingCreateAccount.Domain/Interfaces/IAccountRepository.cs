using OnboardingCreateAccount.Domain.Entities;

namespace OnboardingCreateAccount.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByDocumentAsync(string document);
}
