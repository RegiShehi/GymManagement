using ErrorOr;
using MediatR;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;

namespace GymManagement.Application.Profiles.Commands.CreateAdminProfile;

public class CreateAdminProfileCommandHandler(
    IUserRepository userRepository,
    IAdminRepository adminRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<CreateAdminProfileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateAdminProfileCommand command, CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        if (currentUser.Id != command.UserId)
            return Error.Unauthorized(description: "User is forbidden from taking this action.");

        var user = await userRepository.GetByIdAsync(command.UserId);


        if (user is null) return Error.NotFound(description: "User not found");

        var createAdminProfileResult = user.CreateAdminProfile();
        var admin = new Admin(user.Id, id: createAdminProfileResult.Value);

        await userRepository.UpdateAsync(user);
        await adminRepository.AddAdminAsync(admin);
        await unitOfWork.CommitChangesAsync();

        return createAdminProfileResult;
    }
}