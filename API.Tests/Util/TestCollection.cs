using Xunit;

namespace API.Tests.Util
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}