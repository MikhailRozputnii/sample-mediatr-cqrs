using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;
using UserManagement.Identity.StoreCommands;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class FindAccountByIdHandler : RequestHandler<FindAccountById, User>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;

        public FindAccountByIdHandler(ILogger<FindAccountByIdHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        protected override User Handle(FindAccountById request)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                _logger.LogInformation($"Error. Request with empty Id!");
                return new User();
            }
            var entity = _dbContext.Users.FirstOrDefault(x => x.Id == request.Id);
            if (entity == null)
            {
                _logger.LogInformation($"User with id {request.Id} not found!");
            }
           return entity;
        }
    }
}
