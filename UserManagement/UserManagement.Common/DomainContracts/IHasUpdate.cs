using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.DomainContracts
{
    public interface IHasUpdate
    {
        DateTime UpdateDate { get; set; }
    }
}
