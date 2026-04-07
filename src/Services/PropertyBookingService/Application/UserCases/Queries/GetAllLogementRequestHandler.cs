using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UserCases.Queries
{
    public class GetAllLogementRequestHandler : IRequestHandler<GetAllLogementRequest, List<Logement>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllLogementRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Logement>> Handle(GetAllLogementRequest request, CancellationToken cancellationToken)
        {
            var readRepository = (ILogementReadRepository) _unitOfWork.GetServiceByType(typeof(ILogementReadRepository));
            var logements = await readRepository.ListAsync(cancellationToken);

            return logements.ToList();
        }
    }
}
