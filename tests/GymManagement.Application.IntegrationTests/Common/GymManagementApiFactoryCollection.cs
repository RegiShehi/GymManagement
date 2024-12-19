namespace GymManagement.Application.IntegrationTests.Common;

[CollectionDefinition(CollectionName)]
public class GymManagementApiFactoryCollection : ICollectionFixture<GymManagementApiFactory>
{
    public const string CollectionName = "GymManagementApiFactoryCollection";
}