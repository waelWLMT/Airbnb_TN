using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UserCases.Queries
{
    public class GetLogementByIdRequestHandler : IRequestHandler<GetLogementByIdRequest, Logement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLogementByIdRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Logement> Handle(GetLogementByIdRequest request, CancellationToken cancellationToken)
        {
            var readRepository = _unitOfWork.GetServiceByType(typeof(ILogementReadRepository)) as ILogementReadRepository;
            var logement = await readRepository.GetByIdAsync(request.Id);

            return logement;
        }
    }
}
