using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using MediatR;

namespace GymManagement.Application.Gyms.Events;

public class SubscriptionDeletedEventHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork)
    : INotificationHandler<SubscriptionDeletedEvent>
{
    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var gyms = await gymRepository.ListBySubscriptionIdAsync(notification.SubscriptiodId);

        await gymRepository.RemoveRangeAsync(gyms);
        await unitOfWork.CommitChangesAsync();
    }
}