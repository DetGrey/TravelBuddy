namespace TravelBuddy.Trips
{
    public interface ITripRepositoryFactory
    {
        ITripRepository GetTripRepository();
    }
}