using System.Threading.Tasks;
using EventSourcing;
using SqlStoreTest.Domain;

namespace SqlStoreTest.Application.BirthRegistration
{
    public class RegisterBirthCommandHandler
    {
        private readonly IAsyncRepository<PersonalRecord> _repository;

        public RegisterBirthCommandHandler(IAsyncRepository<PersonalRecord> repository)
        {
            _repository = repository;
        }
        
        public Task Handle(RegisterBirthCommand command)
        {
            var personalRecord = PersonalRecord.ActOfBorn(command.PersonId, command.MotherId, command.FatherId,
                command.Date, command.BornLatitude, command.BornLongitude);

            return _repository.SaveAggregate(personalRecord);
        }
    }
}