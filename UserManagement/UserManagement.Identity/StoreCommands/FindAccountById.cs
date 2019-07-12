using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain;

namespace UserManagement.Identity.StoreCommands
{
    public class FindAccountById:IRequest<User>
    {
        public Guid Id { get; set; }
    }
}
