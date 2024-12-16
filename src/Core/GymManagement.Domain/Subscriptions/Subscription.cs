namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    public Guid Id { get; private set; }
    public SubscriptionType SubscriptionType { get; private set; }
    private readonly Guid _adminId;

    public Subscription(Guid adminId, SubscriptionType subscriptionType, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        _adminId = adminId;
        SubscriptionType = subscriptionType;
    }

    private Subscription()
    {
    }
}