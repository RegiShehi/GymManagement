using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Behaviours;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Domain.Gyms;
using MediatR;
using Moq;
using TestCommon.Gyms;

namespace GymManagement.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourTests
{
    [Fact]
    public async Task InvokeBehaviour_WhenValidatorResultIsValid_ShouldInvokeNextBehaviour()
    {
        // arrange

        // create request
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
        var gym = GymFactory.CreateGym();

        // create next behaviour
        var mockNextBehaviour = new Mock<RequestHandlerDelegate<ErrorOr<Gym>>>();
        mockNextBehaviour.Setup(next => next()).ReturnsAsync(gym);

        // create validator (mock)
        var mockValidator = new Mock<IValidator<CreateGymCommand>>();
        mockValidator
            .Setup(v => v.ValidateAsync(createGymRequest, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // create validation behaviour (SUT)
        var validationBehaviour = new ValidationBehaviour<CreateGymCommand, ErrorOr<Gym>>(mockValidator.Object);

        // act
        var result =
            await validationBehaviour.Handle(createGymRequest, mockNextBehaviour.Object, CancellationToken.None);

        // assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
}