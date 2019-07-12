using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Data;
using UserManagement.Identity.StoreCommands;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class CreateAccountHandler : RequestHandler<CreateAccount, IdentityResult>
    {
        private readonly UserContext _dbContext;

        public CreateAccountHandler(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override IdentityResult Handle(CreateAccount request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                CreateDate = DateTime.UtcNow
            };
            _dbContext.Users.AddAsync(user);
            var result = _dbContext.SaveChanges();
            return result > 0 ? IdentityResult.Success :
                IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }
    }
}
