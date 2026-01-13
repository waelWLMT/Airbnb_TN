using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<Role>?>
    {
        private readonly IRoleReadRepository _roleReadRepository;

        public GetAllRolesQueryHandler(IUnitOfWork unitOfWork)
        {
            _roleReadRepository = unitOfWork.GetRequiredRepository<IRoleReadRepository>();
        }
        public async Task<List<Role>?> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await _roleReadRepository.GetAllRolesAsync(cancellationToken);
        }
    }
}
