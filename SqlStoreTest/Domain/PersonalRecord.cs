using System;
using EventSourcing;
using SqlStoreTest.Domain.Events;

namespace SqlStoreTest.Domain
{
    public class PersonalRecord : Aggregate
    {
        private Guid _personId;
        private bool _isDead;
        private Guid? _spouseId;
        
        public static PersonalRecord ActOfBorn(Guid personId, Guid? motherId, Guid? fatherId, DateTimeOffset date, double? latitude, double? longitude)
            => new PersonalRecord(personId, motherId, fatherId, date, latitude, longitude);
        
        public PersonalRecord()
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

        private PersonalRecord(Guid personId, Guid? motherId, Guid? fatherId, DateTimeOffset date, double? latitude, double? longitude)
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

        public void ActOfMarriage(Guid spouseId, DateTimeOffset when)
        {
            if (_spouseId.HasValue)
                throw new InvalidOperationException("Person has been already married");
            
            ApplyChange(new PersonMarried
            {
                PersonId = _personId,
                SpouseId = spouseId,
                Date = when
            });
        }

        public void ActOfDivorce(DateTimeOffset when)
        {
            if (!_spouseId.HasValue)
                throw new InvalidOperationException("Person is not married"); 
            
            ApplyChange(new PersonDivorced
            {
                PersonId = _personId,
                SpouseId = _spouseId.Value,
                Date = when
            });
        }

        public void ActOfDeath(DateTimeOffset when, double? latitude, double? longitude)
        {
            if (_isDead)
                throw new InvalidOperationException("Person is already dead");
            
            ApplyChange(new PersonDied
            {
                PersonId = _personId,
                Date = when,
                DeathLatitude = latitude,
                DeathLongitude = longitude
            });
        }

    }
}