namespace GymManagement.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; init; }

    protected readonly List<IDomainEvent> DomainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = DomainEvents.ToList();

        DomainEvents.Clear();

        return copy;
    }

    protected Entity()
    {
    }
}