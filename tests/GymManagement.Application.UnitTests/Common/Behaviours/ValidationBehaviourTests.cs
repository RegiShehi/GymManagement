using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Behaviours;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Domain.Gyms;
using MediatR;
using NSubstitute;
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

        // create next behaviour
        var mockNextBehaviour = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();
        var gym = GymFactory.CreateGym();

        mockNextBehaviour.Invoke().Returns(gym);

        // create validator (mock)
        var mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
        mockValidator
            .ValidateAsync(createGymRequest, CancellationToken.None)
            .Returns(new ValidationResult());

        // create validation behaviour (SUT)
        var validationBehaviour = new ValidationBehaviour<CreateGymCommand, ErrorOr<Gym>>(mockValidator);

        // act
        var result = await validationBehaviour.Handle(createGymRequest, mockNextBehaviour, CancellationToken.None);

        // assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
}