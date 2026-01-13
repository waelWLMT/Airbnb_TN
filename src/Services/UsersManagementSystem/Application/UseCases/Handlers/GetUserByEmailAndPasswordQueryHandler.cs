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
                u => u.Email == request.Email && PasswordService.VerifyPassword(request.Password, u.PasswordHash),
                false,
                u => u.UserRole
                );            

            return await _userReadRepository.GetOneAsync(cancellationToken, findOptions);

        }
    }
}
