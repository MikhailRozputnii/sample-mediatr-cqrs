using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using UserManagement.Commands;
using UserManagement.Common.DomainContracts;
using UserManagement.Common.Exceptions;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;

namespace UserManagement.EFDataAccess.Handlers.CommandHandlers
{
    /// <summary>
    /// This class sends a request to the server to delete data, using EntityFramework
    /// </summary>
    public class RemoveUserHandler : RequestHandler<RemoveUser>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public RemoveUserHandler(ILogger<RemoveUserHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Data removing method, sets deletion marker as true
        /// </summary>
        /// <param name="request">command with user id</param>
        protected override void Handle(RemoveUser request)
        {
            try
            {
                var entity = _dbContext.Users.SingleOrDefault(x => x.Id == request.Id);
                if (entity == null)
                {
                    _logger.LogWarning($"User with Id {entity.Id} is not found");
                    throw new NotFoundException("User not found");
                }
                entity.IsDeleted = true;
                _dbContext.SaveChanges();
                _logger.LogInformation($"User {request.Id} is deleted");
            }
            catch (InvalidOperationException ex) {
                _logger.LogWarning($"There are more than two users with such an Id {request.Id}. Trace {ex.StackTrace}");
                throw new InvalidOperationException("There are more than two users with such an Id!");
            }
           
        }
    }
}
