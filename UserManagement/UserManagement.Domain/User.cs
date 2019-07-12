using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Common.Contracts;
using UserManagement.Common.DomainContracts;

namespace UserManagement.Domain
{
    public class User : IEntity, IHasUpdate, IHasCreate, IHasDeleted
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
