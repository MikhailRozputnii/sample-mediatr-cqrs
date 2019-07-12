using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.DomainContracts
{
    public interface IHasCreate
    {
        DateTime CreateDate { get; set; }
    }
}
