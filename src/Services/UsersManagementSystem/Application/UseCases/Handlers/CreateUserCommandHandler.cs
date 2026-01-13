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
using MediatR;

namespace Application.UseCases.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;        

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userWriteRepository = _unitOfWork.GetRequiredRepository<IUserWriteRepository>();            
        }
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            if (request.UserCreateDto == null) throw new ArgumentNullException();

            var user = UserBuilderService.BuildUser(request.UserCreateDto);            
           
            await _userWriteRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return user;

        }
        
    }
}
