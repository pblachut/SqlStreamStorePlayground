using System;

namespace SqlStoreTest.Application.BirthRegistration
{
    public class RegisterBirthCommand
    {
        public Guid PersonId { get; set; }
        
        public Guid? MotherId { get; set; }
        public Guid? FatherId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double? BornLongitude { get; set; }
        public double? BornLatitude { get; set; }
    }
}