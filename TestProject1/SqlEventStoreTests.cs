using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SqlStoreTest;
using SqlStoreTest.Domain.Events;
using Xunit;

namespace TestProject1
{
    [Collection(MsSqlStreamStoreV3FixtureCollection.Name)]
    public class SqlEventStoreTests
    {
        private readonly MsSqlStreamStoreV3Fixture _fixture;

        public SqlEventStoreTests(MsSqlStreamStoreV3Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task can_save_events_in_stream()
        {
            // PREPARE
            var personWasBorn = new PersonWasBorn
            {
                PersonId = Guid.NewGuid(),
                Date = new DateTime(2019, 10, 2)
            };

            var personDied = new PersonDied
            {
                PersonId = Guid.NewGuid(),
                Date = new DateTime(2020, 10, 2)
            };
            
            var fixture = await new Fixture(_fixture).Configure();

            // RUN
            await fixture.Sut.CreateStream("testStream", new List<object> {personWasBorn, personDied});
            
            // ASSERT
            var events = new List<object>();
            await foreach(var @event in fixture.Sut.GetEventsFromStream("testStream"))
            {
                events.Add(@event);
            }
            
            events.Should().BeEquivalentTo(personWasBorn, personDied);
        }
        
        [Fact]
        public async Task can_append_stream()
        {
            // PREPARE
            var personWasBorn = new PersonWasBorn
            {
                PersonId = Guid.NewGuid(),
                Date = new DateTime(2019, 10, 2)
            };

            var personDied = new PersonDied
            {
                PersonId = Guid.NewGuid(),
                Date = new DateTime(2020, 10, 2)
            };
            
            var fixture = await new Fixture(_fixture).Configure();
            await fixture.Sut.CreateStream("testStream", new List<object> {personWasBorn});

            // RUN
            await fixture.Sut.AppendStream("testStream", new List<object> {personDied}, 0);
            
            // ASSERT
            var events = new List<object>();
            await foreach(var @event in fixture.Sut.GetEventsFromStream("testStream"))
            {
                events.Add(@event);
            }
            
            events.Should().BeEquivalentTo(personWasBorn, personDied);
        }
        
        private class Fixture
        {
            private readonly MsSqlStreamStoreV3Fixture _fixture;

            public SqlEventStore Sut;
            
            public Fixture(MsSqlStreamStoreV3Fixture fixture)
            {
                _fixture = fixture;
                
                Sut = new SqlEventStore(_fixture.MsqlStreamStore);
                
                TypeCache.Add<PersonDied>("TestDomain.PersonDied");
                TypeCache.Add<PersonWasBorn>("TestDomain.PersonWasBorn");
            }

            public async Task<Fixture> Configure()
            {
                await _fixture.ClearStore();

                return this;
            }
        }
    }

    
}