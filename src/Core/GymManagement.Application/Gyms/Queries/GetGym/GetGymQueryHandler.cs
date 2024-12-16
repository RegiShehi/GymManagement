using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym;

public class GetGymQueryHandler(IGymRepository gymRepository, ISubscriptionRepository subscriptionRepository)
    : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        if (await subscriptionRepository.ExistsAsync(request.SubscriptionId))
            return Error.NotFound("Subscription not found");

        if (await gymRepository.GetByIdAsync(request.GymId) is not Gym gym)
            return Error.NotFound(description: "Gym not found");

        return gym;
    }
}