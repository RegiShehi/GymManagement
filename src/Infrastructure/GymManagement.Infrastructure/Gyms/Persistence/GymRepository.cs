using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Gyms.Persistence;

public class GymRepository(GymManagementDbContext dbContext) : IGymRepository
{
    public async Task AddGymAsync(Gym gym)
    {
        await dbContext.Gyms.AddAsync(gym);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await dbContext.Gyms.AsNoTracking().AnyAsync(gym => gym.Id == id);
    }

    public async Task<Gym?> GetByIdAsync(Guid id)
    {
        return await dbContext.Gyms.FirstOrDefaultAsync(gym => gym.Id == id);
    }

    public async Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await dbContext.Gyms
            .Where(gym => gym.SubscriptionId == subscriptionId)
            .ToListAsync();
    }

    public Task RemoveGymAsync(Gym gym)
    {
        dbContext.Remove(gym);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(List<Gym> gyms)
    {
        dbContext.RemoveRange(gyms);

        return Task.CompletedTask;
    }

    public Task UpdateGymAsync(Gym gym)
    {
        dbContext.Update(gym);

        return Task.CompletedTask;
    }
}