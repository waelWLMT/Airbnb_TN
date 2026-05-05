using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }
        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userReadRepository.GetByIdAsync(request.Id, cancellationToken);  
        }
    }
}
