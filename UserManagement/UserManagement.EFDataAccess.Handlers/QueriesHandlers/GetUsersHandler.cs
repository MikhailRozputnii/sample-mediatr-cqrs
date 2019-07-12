using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Commands;
using UserManagement.Common;
using UserManagement.Domain;
using UserManagement.EFDataAccess.Handlers.Common;
using UserManagement.EFDataAccess.Data;

namespace UserManagement.EFDataAccess.Handlers.QueriesHandlers
{
    /// <summary>
    /// This class sends a request to the server to getting data, using EntityFramework technology
    /// </summary>
    public class GetUsersHandler :RequestHandler<GetUsers, (IEnumerable<User>, int)>
    {
        private readonly ILogger _logger;
        private readonly UserContext _dbContext;
        public GetUsersHandler(ILogger<GetUsersHandler> logger, UserContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method sends a request to the server to getting  data using pagination
        /// </summary>
        /// <param name="request"> command with property for using pagination </param>
        /// <returns>collections user data, and count user data</returns>
        protected override (IEnumerable<User>, int) Handle(GetUsers request)
        {
            _logger.LogInformation($"Get users");
            IQueryable<User> users = _dbContext.Users;
            var getUsers = new UsersQueryBuilder(users);
            var getTotal = new UsersQueryBuilder(users);
            if (!string.IsNullOrEmpty(request.Search))
            {
                _logger.LogInformation($"Search users");
                getUsers.FindUser(request.Search);
                getTotal.FindUser(request.Search);
            }

            var total = getTotal.GetResult().Count();

            getUsers.SortUsers(request);
            getUsers.FilterPage(request);
            var usersList = getUsers.GetResult().ToList();
            return (usersList, total);
        }

        private IQueryable<User> Sort(ISortProperty sort, IQueryable<User> users)
        {
            switch (sort.SortField)
            {
                case SortField.Name:
                    return sort.IsAscending ? users.OrderBy(o => o.FirstName) : users.OrderByDescending(o => o.FirstName);
                case SortField.Email:
                    return sort.IsAscending ? users.OrderBy(o => o.Email) : users.OrderByDescending(o => o.Email);
                default:
                    return sort.IsAscending ? users.OrderBy(o => o.Id) : users.OrderByDescending(o => o.Id);
            }
        }
    }
}
