using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;
using UserManagement.Identity.StoreCommands;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class FindAccountByEmailHandler : RequestHandler<FindAccountByEmail, User>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public FindAccountByEmailHandler(ILogger<FindAccountByEmailHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override User Handle(FindAccountByEmail request)
        {
            var user = _dbContext.Users.FirstOrDefault(n => n.Email.Equals(request.Email));
            if (user == null)
            {
                _logger.LogInformation($"User with {request.Email} not found!");
            }
            return user;
        }
    }
}
