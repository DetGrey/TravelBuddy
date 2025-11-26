namespace TravelBuddy.Trips
{
    public interface ITripRepositoryFactory
    {
        ITripDestinationRepository GetTripDestinationRepository();
        IBuddyRepository GetBuddyRepository();
    }
}