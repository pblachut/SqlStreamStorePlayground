using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SqlStoreTest.Application.BirthRegistration
{
    [ApiController]
    [Route("api")]
    public class RegisterBirthApi : Controller
    {
        private readonly RegisterBirthCommandHandler _commandHandler;

        public RegisterBirthApi(RegisterBirthCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }
        
        [HttpPost]
        [Route("registerBirth")]
        public async Task<Guid> RegisterBirth(RegisterBirthRequest request)
        {
            var command = new RegisterBirthCommand
            {
                PersonId = Guid.NewGuid(),
                MotherId = request.MotherId,
                FatherId = request.FatherId,
                Date = request.Date,
                BornLatitude = request.BornLatitude,
                BornLongitude = request.BornLongitude
            };

            await _commandHandler.Handle(command);

            return command.PersonId;
        }
    }
}