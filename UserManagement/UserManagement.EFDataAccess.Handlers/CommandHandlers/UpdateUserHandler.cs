using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using UserManagement.Commands;
using UserManagement.Common.Exceptions;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;

namespace UserManagement.EFDataAccess.Handlers.CommandHandlers
{
    /// <summary>
    /// This class sends a request to the server to update data, using EntityFramework
    /// </summary>
    public class UpdateUserHandler : RequestHandler<UpdateUser>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public UpdateUserHandler(ILogger<UpdateUserHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method sends a request to the server to update the data
        /// </summary>
        /// <param name="request">command with user properties</param>
        protected override void Handle(UpdateUser request)
        {
            User user = null;
            try
            {
                user = _dbContext.Users.SingleOrDefault(x => x.Id == request.Id);
                if (user == null)
                {
                    _logger.LogWarning($"User with Id {request.Id} is not found");
                    throw new NotFoundException($"User with Id {request.Id} is not found");
                }
                var userByEmail = _dbContext.Users
                    .FirstOrDefault(o => o.Email.Equals(request.Email));

                if (userByEmail != null && user.Id != userByEmail.Id)
                {
                    _logger.LogWarning("User with this email already exists");
                    throw new ModelValidationException("User with this email already exists");
                }
            }
            catch (InvalidOperationException ex) {
                _logger.LogWarning($"There are more than two users with such an Id {request.Id}. Trace {ex.StackTrace}");
                throw new InvalidOperationException("There are more than two users with such an Id!");
            }
            
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UpdateDate = DateTime.UtcNow;

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            _logger.LogInformation($"User {user.Id} is update");
        }
    }
}
