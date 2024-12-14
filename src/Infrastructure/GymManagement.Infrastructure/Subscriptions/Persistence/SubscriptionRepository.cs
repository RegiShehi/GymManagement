using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionRepository(GymManagementDbContext dbContext) : ISubscriptionRepository
{
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await dbContext.Subscriptions.FindAsync(subscriptionId);
    }
}