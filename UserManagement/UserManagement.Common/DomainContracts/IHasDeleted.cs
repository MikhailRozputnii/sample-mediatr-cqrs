using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.DomainContracts
{
    public interface  IHasDeleted
    {
       bool IsDeleted { get; set; }
    }
}
