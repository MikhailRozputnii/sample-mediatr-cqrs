using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain;

namespace UserManagement.Commands
{
    public class GetUserByEmail: IRequest<User>
    {
        public string Email { get; set; }
    }
}
