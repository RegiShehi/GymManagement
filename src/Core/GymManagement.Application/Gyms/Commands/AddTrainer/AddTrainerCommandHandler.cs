using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainer;

public class AddTrainerCommandHandler(
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AddTrainerCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByIdAsync(command.GymId);

        if (gym is null) return Error.NotFound(description: "Gym not found");

        var addTrainerResult = gym.AddTrainer(command.TrainerId);

        if (addTrainerResult.IsError) return addTrainerResult.Errors;

        await gymRepository.UpdateGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Success;
    }
}