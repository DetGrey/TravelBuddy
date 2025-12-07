using Xunit;

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<TravelBuddyApiFactory<Program>>
{
    // This class has no code. It never gets instantiated.
    // Its purpose is to apply the [CollectionDefinition] and the ICollectionFixture interface.
}