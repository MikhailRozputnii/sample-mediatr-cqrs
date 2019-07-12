using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;
using UserManagement.Identity.StoreCommands;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class UpdateAccountHandler : RequestHandler<UpdateAccount, IdentityResult>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public UpdateAccountHandler(ILogger<UpdateAccountHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        protected override IdentityResult Handle(UpdateAccount request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                UpdateDate = DateTime.UtcNow
            };

            _dbContext.Attach(user);
            _dbContext.Update(user);
            try
            {
               _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning($"Error with update account. Trace {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = $"Error with update account" });
            }
            return IdentityResult.Success;
        }
    }
}
