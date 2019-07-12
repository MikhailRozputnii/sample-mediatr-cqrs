using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManagement.Common;
using UserManagement.Common.Contracts;
using UserManagement.Domain;

namespace UserManagement.EFDataAccess.Handlers.Common
{
    public class UsersQueryBuilder
    {
        private IQueryable<User> _users=null;
        public UsersQueryBuilder(IQueryable<User> users)
        {
            _users = users;
        }

        public void FindUser(string search)
        {
           _users = _users.Where(u => u.FirstName.Contains(search) ||
                       u.LastName.Contains(search) ||
                       u.Email.Contains(search));
        }

        public void SortUsers(ISortProperty sort)
        {
            switch (sort.SortField)
            {
                case SortField.Name:
                    _users = sort.IsAscending ? _users.OrderBy(o => o.FirstName) : _users.OrderByDescending(o => o.FirstName);
                    break;
                case SortField.Email:
                    _users = sort.IsAscending ? _users.OrderBy(o => o.Email) : _users.OrderByDescending(o => o.Email);
                    break;
                default:
                    _users = _users.OrderBy(o => o.Id);
                    break;
            }
        }

        public void FilterPage(IPaging paging)
        {
            _users = _users.Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize);
        }

        public IQueryable<User> GetResult() {
            return _users;
        }
    }
}
