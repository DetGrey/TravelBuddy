namespace TravelBuddy.Users
{
    public interface IUserRepositoryFactory
    {
        /// Retrieves the correct, request-scoped IUserRepository instance 
        /// (MySql, MongoDb, or Neo4j).
        IUserRepository GetUserRepository();
    }
}