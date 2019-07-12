using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain;

namespace UserManagement.Identity.StoreCommands
{
    public class FindAccountByEmail:IRequest<User>
    {
        public string Email { get; set; }
    }
}
