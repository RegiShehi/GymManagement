using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandBehaviour : IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand request, RequestHandlerDelegate<ErrorOr<Gym>> next,
        CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.Errors.Select(error => Error.Validation(error.PropertyName, error.ErrorMessage))
                .ToList();

        return await next();
    }
}