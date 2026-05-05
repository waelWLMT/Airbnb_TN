using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Commands;
using Contracts.Events;
using Contracts.Events.Proprietaires;
using Contracts.Events.Voyageurs;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepositroy;
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public ActivateUserCommandHandler(IUnitOfWork unitOfWork, IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepositroy, IOutboxMessageRepository outboxMessageRepository)
        {
            _unitOfWork = unitOfWork;
            _userReadRepository = userReadRepository;
            _userWriteRepositroy = userWriteRepositroy;
            _outboxMessageRepository = outboxMessageRepository;
        }
        public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(request.Id);

            if (user == null)
                return false;

            user.IsActive = request.ActivateUser;
            await _userWriteRepositroy.UpdateAsync(user);

            if (user.RoleId == (int)UserRole.Admin)
            {
                await _unitOfWork.CommitAsync();
                return true;
            }


            var outboxMessage = user.RoleId == (int)UserRole.Voyageur
                                                                    ? OutBoxMessageBuilder.BuildVoyageurActivatedMessage(user)
                                                                    : OutBoxMessageBuilder.BuildProprietaireActivatedMessage(user);

            await _outboxMessageRepository.AddAsync(outboxMessage);

            await _unitOfWork.CommitAsync();

            return true;

        }
    }
}
