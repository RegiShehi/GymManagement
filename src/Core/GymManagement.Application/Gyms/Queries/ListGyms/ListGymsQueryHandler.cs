using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.ListGyms;

public class ListGymsQueryHandler(IGymRepository gymRepository, ISubscriptionRepository subscriptionRepository)
    : IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery query, CancellationToken cancellationToken)
    {
        if (!await subscriptionRepository.ExistsAsync(query.SubscriptionId))
            return Error.NotFound(description: "Subscription not found");

        return await gymRepository.ListBySubscriptionIdAsync(query.SubscriptionId);
    }
}