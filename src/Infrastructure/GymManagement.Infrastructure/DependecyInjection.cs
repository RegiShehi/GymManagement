using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        
        return services;
    }
}