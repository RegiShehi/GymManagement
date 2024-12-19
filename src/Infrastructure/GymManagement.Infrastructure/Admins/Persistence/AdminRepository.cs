using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Admins.Persistence;

public class AdminRepository(GymManagementDbContext dbContext) : IAdminRepository
{
    public async Task AddAdminAsync(Admin admin)
    {
        await dbContext.Admins.AddAsync(admin);
    }

    public async Task<Admin?> GetByIdAsync(Guid adminId)
    {
        return await dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == adminId);
    }

    public Task UpdateAsync(Admin admin)
    {
        dbContext.Admins.Update(admin);

        return Task.CompletedTask;
    }
}