using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using UserManagement.Common.Exceptions;
using UserManagement.EFDataAccess.Data;
using UserManagement.Identity.StoreCommands;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class DeleteAccountHandler : RequestHandler<DeleteAccount, IdentityResult>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;

        public DeleteAccountHandler(ILogger<DeleteAccountHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        protected override IdentityResult Handle(DeleteAccount request)
        {
            var entity =  _dbContext.Users.FirstOrDefault(x => x.Id == request.Id);
            if (entity == null)
            {
                _logger.LogWarning($"User with id {request.Id} not found!");
                throw new NotFoundException($"User {request.Id} not found");
            }
            _dbContext.Users.Remove(entity);
            var result = _dbContext.SaveChanges();

            return result > 0 ? IdentityResult.Success :
                IdentityResult.Failed(new IdentityError { Description = "Could not delete user" });
        }
    }
}
