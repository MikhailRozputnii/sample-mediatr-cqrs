using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain;

namespace UserManagement.Commands
{
    public class GetUser:IRequest<User>
    {
        public Guid Id { get; set; }
    }
}
