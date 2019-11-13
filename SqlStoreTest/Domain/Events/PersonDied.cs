using System;

namespace SqlStoreTest.Domain.Events
{
    public class PersonDied
    {
        public Guid PersonId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double? DeathLongitude { get; set; }
        public double? DeathLatitude { get; set; }
    }
}