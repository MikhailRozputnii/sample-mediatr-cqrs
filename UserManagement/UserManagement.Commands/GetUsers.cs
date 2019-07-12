using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Common;
using UserManagement.Common.Contracts;
using UserManagement.Domain;

namespace UserManagement.Commands
{
    public class GetUsers : IRequest<(IEnumerable<User>, int)>, IPaging, ISortProperty
    {
        public string Search { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public bool IsAscending { get; set; }
    }
}
