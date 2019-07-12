using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Commands;
using UserManagement.Common.Exceptions;
using UserManagement.Domain;

namespace UserManagement.EFDataAccess.Handlers.IdentityHandlers
{
    public class RegisterUserHandler : AsyncRequestHandler<RegisterUser>
    {
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public RegisterUserHandler(ILogger<RegisterUserHandler> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        protected override async Task Handle(RegisterUser request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CreateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else
            {
                _logger.LogWarning($"Registration error!");
                throw new ModelValidationException($"Registration error! {result}");
            }
        }
    }
}
