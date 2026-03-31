using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingCreateAccount.Domain.Interfaces;
using OnboardingCreateAccount.Infrastructure.Context;
using OnboardingCreateAccount.Infrastructure.Repositories;
using OnboardingCreateAccount.Infrastructure.Cache;
using OnboardingCreateAccount.Infrastrsucture.Context;

namespace OnboardingCreateAccount.Infrastructure.Context;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddStackExchangeRedisCache(options =>
        {
           options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "KrtBank_";
        });

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
