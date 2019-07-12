using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common
{
    public interface ISortProperty
    {
        string SortField { get; set; }
        bool IsAscending { get; set; }
    }
}
