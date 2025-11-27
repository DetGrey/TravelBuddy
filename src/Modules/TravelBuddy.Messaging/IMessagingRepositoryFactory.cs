namespace TravelBuddy.Messaging
{
    public interface IMessagingRepositoryFactory
    {
        IMessagingRepository GetMessagingRepository();
    }
}