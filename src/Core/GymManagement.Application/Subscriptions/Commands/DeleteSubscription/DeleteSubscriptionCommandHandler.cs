using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IGymRepository _gymRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubscriptionCommandHandler(
        IAdminRepository adminRepository,
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork,
        IGymRepository gymRepository)
    {
        _adminRepository = adminRepository;
        _subscriptionRepository = subscriptionRepository;
        _unitOfWork = unitOfWork;
        _gymRepository = gymRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null) return Error.NotFound(description: "Subscription not found");

        var admin = await _adminRepository.GetByIdAsync(subscription.AdminId);

        if (admin is null) return Error.Unexpected(description: "Admin not found");

        admin.DeleteSubscription(command.SubscriptionId);

        var gymsToDelete = await _gymRepository.ListBySubscriptionIdAsync(command.SubscriptionId);

        await _adminRepository.UpdateAsync(admin);
        await _subscriptionRepository.RemoveSubscriptionAsync(subscription);
        await _gymRepository.RemoveRangeAsync(gymsToDelete);
        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}