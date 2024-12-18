﻿using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Domain.Subscriptions;
using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

public static class SubscriptionCommandFactory
{
    public static CreateSubscriptionCommand CreateCreateSubscriptionCommand(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null)
    {
        return new CreateSubscriptionCommand(
            subscriptionType ?? Constants.Subscription.DefaultSubscriptionType,
            adminId ?? Constants.Admin.Id);
    }
}