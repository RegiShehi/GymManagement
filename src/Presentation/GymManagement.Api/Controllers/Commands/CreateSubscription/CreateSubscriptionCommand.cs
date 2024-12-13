using MediatR;

namespace GymManagement.Api.Controllers.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubscriptionType, Guid AdminId) : IRequest<Guid>;
