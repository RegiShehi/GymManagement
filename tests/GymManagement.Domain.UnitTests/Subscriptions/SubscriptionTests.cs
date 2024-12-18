using ErrorOr;
using FluentAssertions;
using GymManagement.Domain.Subscriptions;
using GymManagement.Domain.UnitTests.Gyms;
using TestCommon.Subscriptions;

namespace GymManagement.Domain.UnitTests.Subscriptions;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var subscription = SubscriptionFactory.CreateSubscription();

        var gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
            .Select(_ => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();

        // act
        var gymResults = gyms.ConvertAll(x => subscription.AddGym(x));
        var allButLastGymResults = gymResults[..^1];
        var lastGymResult = gymResults.Last();

        // assert
        allButLastGymResults.Should().AllSatisfy(x => x.Value.Should().Be(Result.Success));
        allButLastGymResults.Should().AllBeOfType<ErrorOr<Success>>();

        lastGymResult.IsError.Should().BeTrue();
        lastGymResult.FirstError.Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
        lastGymResult.FirstError.Should().BeOfType<Error>();
    }
}