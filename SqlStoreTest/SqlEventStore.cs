using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventSourcing;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace SqlStoreTest
{
    public class SqlEventStore : IEventStore
    {
        private readonly IStreamStore _streamStore;

        public SqlEventStore(IStreamStore streamStore)
        {
            _streamStore = streamStore;
        }

        public Task CreateStream(string streamId, List<object> events)
            => _streamStore.AppendToStream(new StreamId(streamId), ExpectedVersion.NoStream, CreateMessages(events));

        public Task AppendStream(string streamId, List<object> events, int expectedVersion)
            => _streamStore.AppendToStream(new StreamId(streamId), expectedVersion, CreateMessages(events));

        public async IAsyncEnumerable<object> GetEventsFromStream(string streamId)
        {
            var stream = new StreamId(streamId);
            ReadStreamPage page;

            var readFrom = StreamVersion.Start;
            do
            {
                page = await _streamStore.ReadStreamForwards(stream, readFrom, 1000);

                yield return GetEvents(page.Messages);

                readFrom = page.NextStreamVersion;
            } while (page.IsEnd);

            async IAsyncEnumerable<object> GetEvents(StreamMessage[] messages)
            {
                foreach (var message in messages)
                {
                    var json = await message.GetJsonData();
                    yield return Deserialize(TypeCache.GetType(message.Type), json);
                }
            }
        }

        public async Task<int> GetLastEventNumber(string streamId)
        {
            var result = await _streamStore.ReadStreamBackwards(new StreamId(streamId), StreamVersion.End, 1);

            return result.LastStreamVersion;
        }

        NewStreamMessage[] CreateMessages(IEnumerable<object> events)
            => events
                .Select(e => new NewStreamMessage(Guid.NewGuid(), TypeCache.GetName(e.GetType()), Serialize(e)))
                .ToArray();

        string Serialize(object objectToSerialize) => JsonSerializer.Serialize(objectToSerialize);
        object Deserialize(Type destinationType, string json) => JsonSerializer.Deserialize(json, destinationType);
    }
}