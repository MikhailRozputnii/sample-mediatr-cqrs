using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.Contracts
{
    public interface IPaging
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }
}
