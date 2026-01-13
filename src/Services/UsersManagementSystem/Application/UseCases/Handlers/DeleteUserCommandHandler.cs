using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Commands;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = _unitOfWork.GetRequiredRepository<IUserWriteRepository>();
            _userReadRepository = _unitOfWork.GetRequiredRepository<IUserReadRepository>();
        }

        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(command.Id, cancellationToken);

            if (user == null) throw new KeyNotFoundException("User not found");

            await _userWriteRepository.DeleteAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;

        }
    }
}
