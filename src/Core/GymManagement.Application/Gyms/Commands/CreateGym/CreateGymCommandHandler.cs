using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;


namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null) return Error.NotFound(description: "Subscription not found");

        var gym = new Gym(
            command.Name,
            subscription.GetMaxRooms(),
            subscription.Id);

        var addGymResult = subscription.AddGym(gym);

        if (addGymResult.IsError) return addGymResult.Errors;

        await subscriptionRepository.UpdateAsync(subscription);
        await gymRepository.AddGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return gym;
    }
}