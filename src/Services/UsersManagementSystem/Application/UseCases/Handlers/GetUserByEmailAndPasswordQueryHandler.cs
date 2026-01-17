using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class GetUserByEmailAndPasswordQueryHandler : IRequestHandler<GetUserByEmailAndPasswordQuery, User?>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByEmailAndPasswordQueryHandler(IUnitOfWork unitOfWork)
        {
            _userReadRepository = unitOfWork.GetRequiredRepository<IUserReadRepository>();
        }
        public async Task<User?> Handle(GetUserByEmailAndPasswordQuery request, CancellationToken cancellationToken)
        {

            var findOptions = new FindOptions()
                .BuildFindOptions(
                u => u.Email == request.Email,
                false,
                u => u.UserRole
                );

            var users = await _userReadRepository.GetAllAsync(cancellationToken, findOptions);  

            return users.Find(u => PasswordService.VerifyPassword(request.Password, u.PasswordHash));

        }
    }
}
