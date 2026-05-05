using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly IUserReadRepository _userReadRepositroy;

        public GetAllUsersQueryHandler(IUserReadRepository userReadRepositroy)
        {
            _userReadRepositroy = userReadRepositroy;
        }
        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {

            var findOptions = new FindOptions<User>
            {
                IsAsNoTracking = request.IsReadOnly,
                Includes = request.UserWithRole
                                                ? new System.Linq.Expressions.Expression<Func<User, object>>[] { u => u.UserRole }
                                                : Array.Empty<System.Linq.Expressions.Expression<Func<User, object>>>()
            };

            return await _userReadRepositroy.GetAllAsync(cancellationToken, findOptions) ?? new List<User>();
        }
    }
}
