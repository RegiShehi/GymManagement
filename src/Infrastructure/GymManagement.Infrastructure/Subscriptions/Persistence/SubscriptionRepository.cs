using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionRepository : ISubscriptionRepository
{
    private static readonly List<Subscription> Subscriptions = [];
    
    public Task AddSubscriptionAsync(Subscription subscription)
    {
        Subscriptions.Add(subscription);
        
        return Task.CompletedTask;
    }

    public Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        throw new NotImplementedException();
    }
}