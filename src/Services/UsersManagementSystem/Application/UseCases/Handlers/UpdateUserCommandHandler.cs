using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Commands;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public UpdateUserCommandHandler(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
            _userWriteRepository = _unitOfWork.GetRequiredRepository<IUserWriteRepository>();
            _userReadRepository = _unitOfWork.GetRequiredRepository<IUserReadRepository>();
        }
        public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var userToUpdate = await _userReadRepository.GetByIdAsync(request.Id, cancellationToken);

            if (userToUpdate == null) throw new KeyNotFoundException($"User with Id {request.Id} not found.");


            setUserToUpdate(userToUpdate, request.UserUpdateDto);
            await _userWriteRepository.UpdateAsync(userToUpdate, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);


            return userToUpdate;

        }

        private void setUserToUpdate(User userToUpdate, Dtos.UserUpdateDto userUpdateDto)
        {
            userToUpdate.Nom = userUpdateDto.Nom;
            userToUpdate.Prenom = userUpdateDto.Prenom;
            userToUpdate.Email = userUpdateDto.Email;

            if (!string.IsNullOrEmpty(userUpdateDto.Password))
                userToUpdate.PasswordHash = PasswordService.HashPassword(userUpdateDto.Password);

            userToUpdate.RoleId = userUpdateDto.RoleId;
            userToUpdate.UpdatedAt = DateTime.UtcNow;
        }
    }
}
