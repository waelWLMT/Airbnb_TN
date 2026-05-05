using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<Role>?>
    {
        private readonly IRoleReadRepository _roleReadRepository;

        public GetAllRolesQueryHandler(IRoleReadRepository roleReadRepository)
        {
            _roleReadRepository = roleReadRepository;
        }
        public async Task<List<Role>?> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleReadRepository.ListAsync(cancellationToken, false);
            return roles.ToList();
        }
    }
}
