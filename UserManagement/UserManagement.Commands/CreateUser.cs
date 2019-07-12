using MediatR;
using System;

namespace UserManagement.Commands
{
    public class CreateUser: IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
