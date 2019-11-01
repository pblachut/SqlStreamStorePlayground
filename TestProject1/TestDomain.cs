using System;

namespace TestProject1
{
    public class PersonWasBorn
    {
        public Guid PersonId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double? BornLongitude { get; set; }
        public double? BornLatitude { get; set; }
    }

    public class PersonDied
    {
        public Guid PersonId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double? BornLongitude { get; set; }
        public double? BornLatitude { get; set; }
    }
}