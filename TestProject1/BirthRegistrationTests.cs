using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    [Collection(MsSqlStreamStoreV3FixtureCollection.Name)]
    public class BirthRegistrationTests
    {
        private readonly MsSqlStreamStoreV3Fixture _fixture;

        public BirthRegistrationTests(MsSqlStreamStoreV3Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task can_save_events_in_stream()
        {
            // PREPARE
            
        }
        
        private class Fixture
        {
            public Fixture(MsSqlStreamStoreV3Fixture fixture)
            {
                
            }
        }
    }
}