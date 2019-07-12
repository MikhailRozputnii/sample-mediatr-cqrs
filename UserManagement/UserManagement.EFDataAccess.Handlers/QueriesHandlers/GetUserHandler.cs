using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using UserManagement.Commands;
using UserManagement.Common.Exceptions;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;

namespace UserManagement.EFDataAccess.Handlers.QueriesHandlers
{
    /// <summary>
    /// This class sends a request to the server to getting data by id, using EntityFramework technology
    /// </summary>
    public class GetUserHandler : RequestHandler<GetUser, User>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public GetUserHandler(ILogger<GetUserHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method sends a request to the server to getting  data
        /// </summary>
        /// <param name="request">command with user id as Guid object</param>
        /// <returns>domain model object</returns>
        protected override User Handle(GetUser request)
        {
            User user = null;
            try
            {
                user = _dbContext.Users.SingleOrDefault(x => x.Id == request.Id);
                if (user == null)
                {
                    _logger.LogWarning($"User with Id {user.Id} is not found");
                    throw new NotFoundException("User not found");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"There are more than two users with such an Id {request.Id}. Trace {ex.StackTrace}");
                throw new InvalidOperationException("There are more than two users with such an Id!");
            }

            _logger.LogInformation($"Get User by Id {user.Id}");
            return user;
        }
    }
}
