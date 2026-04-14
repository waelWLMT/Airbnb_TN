using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Commands;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Messaging.Events;

namespace Application.UseCases.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = _unitOfWork.GetRequiredRepository<IUserWriteRepository>();
            _publishEndpoint = publishEndpoint;
        }
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var user = UserBuilderService.BuildUser(request.UserCreateDto);            
           
            await _userWriteRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var userCreatedEvent = new UserCreatedEvent(user.Id, user.RoleId);
            await _publishEndpoint.Publish(userCreatedEvent);

            return user;

        }
        
    }
}
