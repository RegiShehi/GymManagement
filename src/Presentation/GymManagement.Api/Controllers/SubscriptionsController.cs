using GymManagement.Application.Services;
using GymManagement.Contracts.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionWriteService _subscriptionWriteService;

    public SubscriptionsController(ISubscriptionWriteService subscriptionWriteService)
    {
        _subscriptionWriteService = subscriptionWriteService;
    }
    
    [HttpPost]
    public IActionResult CreateSubscription(CreateSubscriptionRequest request)
    {
        var subscriptionId = _subscriptionWriteService.CreateSubscription(request.SubscriptionType.ToString(), request.AdminId);

        var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);
        
        return Ok(response);
    }
}