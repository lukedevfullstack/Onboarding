using OnboardingCreateAccount.Domain.Interfaces;

namespace OnboardingCreateAccount.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }
}
