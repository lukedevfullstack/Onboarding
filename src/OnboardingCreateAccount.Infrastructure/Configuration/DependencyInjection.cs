using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Infrastructure.Repositories;
using OnboardingCreateAccount.Infrastructure.Cache;
using OnboardingCreateAccount.Infrastrsucture.Context;

namespace OnboardingCreateAccount.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        services.AddDistributedMemoryCache();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
