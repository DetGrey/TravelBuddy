namespace TravelBuddy.SharedKernel.Domain
{
    // Base class for all entities in the entire application
    public abstract class BaseEntity<TId>
    {
        // TId is the type of the primary key (e.g., int, Guid)
        public TId Id { get; protected set; }

        // We use a protected setter to ensure the ID is managed internally
        // (usually by the database/ORM like EF Core) or by a factory method.
    }
}