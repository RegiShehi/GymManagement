using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = new Subscription(subscriptionType: request.SubscriptionType, adminId: request.AdminId);

        await subscriptionRepository.AddSubscriptionAsync(subscription);

        await unitOfWork.CommitChangesAsync();

        return subscription;
    }
}