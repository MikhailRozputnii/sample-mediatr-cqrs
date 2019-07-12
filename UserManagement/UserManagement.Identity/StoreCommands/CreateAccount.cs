using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagement.Identity.StoreCommands
{
    public class CreateAccount:IRequest<IdentityResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
