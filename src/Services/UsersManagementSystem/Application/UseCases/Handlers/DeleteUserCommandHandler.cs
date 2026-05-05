using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Commands;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IOutboxMessageRepository _outBoxMessageRepository;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IOutboxMessageRepository outboxMessageRepository)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _outBoxMessageRepository = outboxMessageRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(command.Id, cancellationToken);

            if (user == null) throw new KeyNotFoundException("User not found");

            await _userWriteRepository.DeleteAsync(user, cancellationToken);
            
            if(user.RoleId == (int) UserRole.Admin)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }

            var outboxMessage = user.RoleId == (int) UserRole.Voyageur
                    ? OutBoxMessageBuilder.BuildVoyageurDeletedMessage(user)
                    : OutBoxMessageBuilder.BuildProprietaireDeletedMessage(user);

            await _outBoxMessageRepository.AddAsync(outboxMessage, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;

        }
    }
}
