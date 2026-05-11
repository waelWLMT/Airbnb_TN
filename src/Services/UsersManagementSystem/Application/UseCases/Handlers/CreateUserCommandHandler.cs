using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Commands;
using Contracts.Events.Voyageurs;
using Contracts.Events.Proprietaires;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IOutboxMessageWriteRepository _outboxMessageRepository;
        private readonly ICorrelationContext _correlationContext;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork,
            IUserWriteRepository userWriteRepository,
            IOutboxMessageWriteRepository outboxMessageRepository,
            ICorrelationContext correlationContext
            )
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = userWriteRepository;
            _outboxMessageRepository = outboxMessageRepository;
            _correlationContext = correlationContext;
        }
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var user = UserBuilderService.BuildUser(request.UserCreateDto);
            await _userWriteRepository.AddAsync(user, cancellationToken);

            if (user.RoleId == (int) UserRole.Admin)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                return user;
            }

            var outboxMessage = user.RoleId == (int)UserRole.Voyageur
                                                ? OutBoxMessageBuilder.BuildVoyageurCreatedMessage(user, _correlationContext.CorrelationId)
                                                : OutBoxMessageBuilder.BuildProprietaireCreatedMessage(user, _correlationContext.CorrelationId);

            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return user;
        }

    }
}
