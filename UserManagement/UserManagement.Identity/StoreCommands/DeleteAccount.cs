using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Identity.StoreCommands
{
    public class DeleteAccount:IRequest<IdentityResult>
    {
        public Guid Id { get; set; }
    }
}
