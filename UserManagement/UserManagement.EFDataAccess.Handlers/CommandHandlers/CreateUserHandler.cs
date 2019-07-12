using MediatR;
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
    /// This class sends user data to the server, using EntityFramework
    /// </summary>
    public class CreateUserHandler : RequestHandler<CreateUser>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;

        public CreateUserHandler(ILogger<CreateUserHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method sends data to the server
        /// </summary>
        /// <param name="request">command with user properties to add data</param>
        protected override void Handle(CreateUser request)
        {
            var existsUser = _dbContext.Users
                .FirstOrDefault(u => u.Email.Equals(request.Email));

            if (existsUser != null)
            {
                _logger.LogWarning($"User with email {request.Email} is already exists");
                throw new ModelValidationException($"User with email {request.Email} is already exists");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            user.CreateDate = DateTime.UtcNow;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            _logger.LogInformation($"User {user.Id} is created");
        }
    }
}
