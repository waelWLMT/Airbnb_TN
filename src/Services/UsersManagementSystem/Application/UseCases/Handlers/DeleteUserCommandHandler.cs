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
        private readonly IOutboxMessageWriteRepository _outBoxMessageRepository;
        private readonly ICorrelationContext _correlationContext;

        public DeleteUserCommandHandler(
            IUnitOfWork unitOfWork,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            IOutboxMessageWriteRepository outboxMessageRepository,
            ICorrelationContext correlationContext)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _outBoxMessageRepository = outboxMessageRepository;
            _correlationContext = correlationContext;
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
                    ? OutBoxMessageBuilder.BuildVoyageurDeletedMessage(user, _correlationContext.CorrelationId)
                    : OutBoxMessageBuilder.BuildProprietaireDeletedMessage(user, _correlationContext.CorrelationId);

            await _outBoxMessageRepository.AddAsync(outboxMessage, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;

        }
    }
}
