// Assuming this is the base class that gives your entity an 'Id' property.
using TravelBuddy.SharedKernel.Domain; 

namespace TravelBuddy.Users
{
    // The core data object for a User. Inherits 'Id' from BaseEntity<int>.
    public class User : BaseEntity<int> 
    {
        // Property for the user's name. Private setter ensures changes are controlled by business methods.
        public string Name { get; private set; } = null!;

        // Property for email. Private setter ensures controlled changes.
        public string Email { get; private set; } = null!;

        // Property to store the secure, hashed version of the password.
        // It's private to prevent accidental exposure or modification outside of the entity's defined behavior.
        public string PasswordHash { get; private set; } = null!;

        // Property for birthdate.
        public DateTime Birthdate { get; private set; }

        // Flag to indicate if the user account is soft-deleted (hidden, but data preserved).
        public bool IsDeleted { get; private set; }

        // IMPORTANT: Private constructor required by Entity Framework Core (EF Core).
        // EF Core uses this to load data from the database without invoking public constructors.
        private User() { }
    }
}