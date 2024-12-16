using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.DeleteGym;

public class DeleteGymCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByIdAsync(command.GymId);

        if (gym is null) return Error.NotFound(description: "Gym not found");

        var subscription = await subscriptionRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null) return Error.NotFound(description: "Subscription not found");

        if (!subscription.HasGym(command.GymId)) return Error.Unexpected(description: "Gym not found");

        subscription.RemoveGym(command.GymId);

        await subscriptionRepository.UpdateAsync(subscription);
        await gymRepository.RemoveGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}