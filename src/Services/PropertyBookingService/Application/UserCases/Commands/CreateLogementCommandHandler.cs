using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UserCases.Commands
{
    public class CreateLogementCommandHandler : IRequestHandler<CreateLogementCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLogementCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateLogementCommand request, CancellationToken cancellationToken)
        {
            var repository = (IRepository<Logement>) _unitOfWork.GetServiceByType(typeof(IRepository<Logement>));
            var ownerReadRepository = (IOwnerReadRepository) _unitOfWork.GetServiceByType(typeof(IOwnerReadRepository));
            
            var logement = request.ToLogement();
            var owner = await ownerReadRepository.GetByIdAsync(request.OwnerId, cancellationToken);
            
            if (owner == null)            
                throw new Exception($"Owner with ID {request.OwnerId} not found.");
            

            logement.Owner = owner;

            await repository.AddAsync(logement, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return logement.Id;
        }
    }
}
