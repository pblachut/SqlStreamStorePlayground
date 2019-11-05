using System;

namespace SqlStoreTest.Domain.Events
{
    public class SpouseDied
    {
        public Guid PersonId { get; set; }
        public Guid SpouseId { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}