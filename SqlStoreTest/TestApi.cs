using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace SqlStoreTest
{
    [ApiController]
    [Route("api")]
    public class TestApi : Controller
    {
        private readonly IStreamStore _streamStore;

        public TestApi(IStreamStore streamStore)
        {
            _streamStore = streamStore;
        }
        
        [HttpPost]
        [Route("test")]
        public async Task Test()
        {
            var streamId = new StreamId("test");

            var @event = new Test
            {
                Id = 233,
                Name = "some name"
            };

            var json = JsonSerializer.Serialize(@event);
            
            var newStreamMessage = new NewStreamMessage(Guid.NewGuid(), "TestType", json);

            await _streamStore.AppendToStream(streamId, ExpectedVersion.Any, newStreamMessage);

        }
    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}