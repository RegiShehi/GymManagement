using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext(
    DbContextOptions options,
    IHttpContextAccessor httpContextAccessor,
    IPublisher publisher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Gym> Gyms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymManagementDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitChangesAsync()
    {
        // get hold of all domain events
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity.PopDomainEvents())
            .SelectMany(e => e)
            .ToList();

        // store events in http context fpr later use
        AddDomainEventsToOfflineProcessingQueue(domainEvents);

        await base.SaveChangesAsync();
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // fetch queue from Http context or create a new queue if it doesn't exist 
        var domainEventsQueue = httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value) && value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new Queue<IDomainEvent>();

        // add domain events to end of queue
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        // store queue in http context
        httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }
}