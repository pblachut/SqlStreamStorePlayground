using System;
using System.Threading.Tasks;
using SqlStreamStore;
using Xunit;

namespace TestProject1
{
    [CollectionDefinition(Name)]
    public class MsSqlStreamStoreV3FixtureCollection : ICollectionFixture<MsSqlStreamStoreV3Fixture>
    {
        public const string Name = "MsSqlStreamStoreV3FixtureCollection";
    }
    
    public class MsSqlStreamStoreV3Fixture : IDisposable
    {
        public MsSqlStreamStoreV3Fixture()
        {
            var config = new TestConfiguration();
            _dbFixture = new DbFixture(config);
            
            MsqlStreamStore = new MsSqlStreamStoreV3(new MsSqlStreamStoreV3Settings(_dbFixture.ConnectionString));
            MsqlStreamStore.CreateSchemaIfNotExists().Wait();
        }

        private readonly DbFixture _dbFixture;
        public MsSqlStreamStoreV3 MsqlStreamStore { get; }

        public Task ClearStore() => _dbFixture.ClearDatabase();

        public void Dispose()
        {
            MsqlStreamStore?.Dispose();
            _dbFixture?.Dispose();
        }
    }
}