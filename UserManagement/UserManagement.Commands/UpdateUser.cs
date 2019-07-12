using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Commands
{
    public class UpdateUser:IRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
