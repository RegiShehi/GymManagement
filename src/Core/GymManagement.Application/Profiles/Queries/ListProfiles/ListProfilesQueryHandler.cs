using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Profiles.Queries.ListProfiles;

public class ListProfilesQueryHandler(IUserRepository userRepository)
    : IRequestHandler<ListProfilesQuery, ErrorOr<ListProfilesResult>>
{
    public async Task<ErrorOr<ListProfilesResult>> Handle(ListProfilesQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(query.UserId);

        if (user is null) return Error.NotFound(description: "User not found");

        return new ListProfilesResult(user.AdminId, user.ParticipantId, user.TrainerId);
    }
}