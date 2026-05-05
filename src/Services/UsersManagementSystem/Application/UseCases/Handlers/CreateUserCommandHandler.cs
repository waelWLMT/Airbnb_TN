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
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserWriteRepository userWriteRepository, IOutboxMessageRepository outboxMessageRepository)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = userWriteRepository;
            _outboxMessageRepository = outboxMessageRepository;
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
                                                ? OutBoxMessageBuilder.BuildVoyageurCreatedMessage(user)
                                                : OutBoxMessageBuilder.BuildProprietaireCreatedMessage(user);

            await _outboxMessageRepository.AddAsync(outboxMessage, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return user;
        }

    }
}
