using Ardalis.SmartEnum;

namespace GymManagement.Domain.Subscriptions;

using System.Collections.Generic;

public class SubscriptionType : SmartEnum<SubscriptionType>
{
    public static readonly SubscriptionType Free = new(nameof(Free), 0);
    public static readonly SubscriptionType Starter = new(nameof(Starter), 1);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 2);

    private static readonly List<SubscriptionType> AllValues = [Free, Starter, Pro];

    public SubscriptionType(string name, int value) : base(name, value)
    {
    }

    public static string GetAllNames()
    {
        var subscriptionTypeNames = string.Join(", ", AllValues
            .Select(type => $"'{type.Name}'"));

        return subscriptionTypeNames;
    }
}