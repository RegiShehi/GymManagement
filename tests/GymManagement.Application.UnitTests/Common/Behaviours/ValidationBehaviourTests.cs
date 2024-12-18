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
    private readonly Mock<RequestHandlerDelegate<ErrorOr<Gym>>> _mockNextBehaviour;
    private readonly Mock<IValidator<CreateGymCommand>> _mockValidator;
    private readonly ValidationBehaviour<CreateGymCommand, ErrorOr<Gym>> _validationBehaviour;

    public ValidationBehaviourTests()
    {
        // create next behaviour
        _mockNextBehaviour = new Mock<RequestHandlerDelegate<ErrorOr<Gym>>>();

        // create validator (mock)
        _mockValidator = new Mock<IValidator<CreateGymCommand>>();

        // create validation behaviour
        _validationBehaviour = new ValidationBehaviour<CreateGymCommand, ErrorOr<Gym>>(_mockValidator.Object);
    }

    [Fact]
    public async Task InvokeBehaviour_WhenValidatorResultIsValid_ShouldInvokeNextBehaviour()
    {
        // arrange
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
        var gym = GymFactory.CreateGym();

        _mockNextBehaviour.Setup(next => next()).ReturnsAsync(gym);

        _mockValidator
            .Setup(v => v.ValidateAsync(createGymRequest, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result =
            await _validationBehaviour.Handle(createGymRequest, _mockNextBehaviour.Object, CancellationToken.None);

        // assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
}