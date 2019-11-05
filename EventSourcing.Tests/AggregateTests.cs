using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Tests.TestDomain;
using Xunit;

namespace EventSourcing.Tests
{
    public class AggregateTests
    {
        [Fact]
        public void Should_Have_Single_Event_To_Persist()
        {
            // PREPARE
            // RUN
            var testAggregate = new TestAggregate(3232, "some name");

            // ASSERT
            Assert.True(testAggregate.AggregateIsNotPersisted());
            Assert.Equal(-1, testAggregate.AggregateVersion);
            Assert.Equal("3232", testAggregate.AggregateId);
            var changes = testAggregate.GetChanges();
            Assert.Single(changes);
            var testAggregateCreated = (TestAggregateCreated) changes.Single();
            Assert.Equal(3232, testAggregateCreated.TestAggregateId);
            Assert.Equal("some name", testAggregateCreated.Name);
        }

        [Fact]
        public async Task Can_Initialize_Aggregate_From_Events()
        {
            // PREPARE
            var @event = new TestAggregateCreated
            {
                TestAggregateId = 332,
                Name = "ddsd"
            };

            // RUN
            var aggregate = new TestAggregate();
            await aggregate.Initialize(GetEvents(@event), Task.FromResult(3333));
            
            // ASSERT
            Assert.False(aggregate.HasChanges());
            Assert.Equal("332", aggregate.AggregateId);
            Assert.Equal(3333, aggregate.AggregateVersion);
            var changes = aggregate.GetChanges();
            Assert.Empty(changes);
        }

        private async IAsyncEnumerable<object> GetEvents(params object[] events)
        {
            await Task.Delay(10);
            foreach (var e in events)
            {
                yield return e;
            }
        }
        
        [Fact]
        public async Task Can_Modify_Aggregate()
        {
            // PREPARE
            var @event = new TestAggregateCreated
            {
                TestAggregateId = 332,
                Name = "ddsd"
            };
            
            var aggregate = new TestAggregate();
            await aggregate.Initialize(GetEvents(@event), Task.FromResult(3333));
            
            // RUN
            aggregate.ChangeName("changed name");
            
            // ASSERT
            Assert.True(aggregate.HasChanges());
            Assert.Equal("332", aggregate.AggregateId);
            Assert.Equal(3333, aggregate.AggregateVersion);
            var changes = aggregate.GetChanges();
            Assert.Single(changes);
            var nameChanged = (TestAggregateNameChanged) changes.Single();
            Assert.Equal(332, nameChanged.TestAggregateId);
            Assert.Equal("ddsd", nameChanged.PreviousName);
            Assert.Equal("changed name", nameChanged.CurrentName);
        }
    }
}