using GymManagement.Api.Extensions;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController(ISender mediator) : ControllerBase
{
    [HttpGet("{subscriptionId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);

        var getSubscriptionsResult = await mediator.Send(query);

        return getSubscriptionsResult.MatchFirst(
            subscription =>
                Ok(new SubscriptionResponse(subscription.Id,
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
            error => error.ToProblem());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid subscription type");

        var command = new CreateSubscriptionCommand(subscriptionType, request.AdminId);

        var createSubscriptionResult = await mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
            error => error.ToProblem()
        );
    }
}