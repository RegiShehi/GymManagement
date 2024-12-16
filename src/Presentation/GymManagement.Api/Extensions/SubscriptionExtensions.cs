using GymManagement.Contracts.Subscriptions;
using GymManagement.Domain.Subscriptions;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;
using ContractSubscriptionType = GymManagement.Contracts.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Extensions;

public static class SubscriptionExtensions
{
    public static SubscriptionResponse ToSubscriptionResponse(this Subscription subscription)
    {
        return new SubscriptionResponse(
            subscription.Id,
            subscription.SubscriptionType.Name switch
            {
                nameof(DomainSubscriptionType.Free) => ContractSubscriptionType.Free,
                nameof(DomainSubscriptionType.Starter) => ContractSubscriptionType.Starter,
                nameof(DomainSubscriptionType.Pro) => ContractSubscriptionType.Pro,
                _ => throw new InvalidOperationException()
            }
        );
    }
}