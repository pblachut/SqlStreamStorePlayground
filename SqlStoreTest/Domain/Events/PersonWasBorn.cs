using System;

namespace SqlStoreTest.Domain.Events
{
    public class PersonWasBorn
    {
        public Guid PersonId { get; set; }
        
        public Guid? MotherId { get; set; }
        public Guid? FatherId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double? BornLongitude { get; set; }
        public double? BornLatitude { get; set; }
    }
}