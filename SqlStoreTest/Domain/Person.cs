using System;
using EventSourcing;
using SqlStoreTest.Domain.Events;

namespace SqlStoreTest.Domain
{
    public class Person : Aggregate
    {
        private Guid _personId;
        private bool _isDead;
        private Guid? _spouseId;
        
        public static Person ActOfBorn(Guid personId, Guid? motherId, Guid? fatherId, DateTimeOffset date, double? latitude, double? longitude)
            => new Person(personId, motherId, fatherId, date, latitude, longitude);
        
        public Person()
        {
            On<PersonWasBorn>(e =>
            {
                AggregateId = e.PersonId.ToString();
                _personId = e.PersonId;
            });
            On<PersonDied>(e => _isDead = true);
            On<PersonMarried>(e => _spouseId = e.SpouseId);
            On<PersonDivorced>(e => _spouseId = null);
            On<SpouseDied>(e => _spouseId = null);
        }

        private Person(Guid personId, Guid? motherId, Guid? fatherId, DateTimeOffset date, double? latitude, double? longitude)
        : this()
            => ApplyChange(new PersonWasBorn
            {
                PersonId = personId,
                MotherId = motherId,
                FatherId = fatherId,
                Date = date,
                BornLatitude = latitude,
                BornLongitude = longitude
            });

    }
}