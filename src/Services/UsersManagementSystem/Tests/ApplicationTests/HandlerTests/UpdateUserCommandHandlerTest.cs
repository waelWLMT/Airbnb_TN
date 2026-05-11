using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Commands;
using Application.UseCases.Handlers;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Tests.Helpers;

namespace UnitTests.ApplicationTests.HandlerTests
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;
        private readonly Mock<IUserWriteRepository> _moqUserWriteRepository;
        private readonly Mock<IUserReadRepository> _moqUserReadRepository;

        public UpdateUserCommandHandlerTest()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>();
            _moqUserWriteRepository = new Mock<IUserWriteRepository>();
            _moqUserReadRepository = new Mock<IUserReadRepository>();
        }

        [Fact]
        public async Task UpdateUserCommandHandler_ShouldUpdateUserSuccessfully()
        {
            // Arrange
            var userToUpdate = UsersTestData.GetFakeUserUpdateDto();

            _moqUserReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UsersTestData.GetSampleUser());

            _moqUserWriteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Models.User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(),
                UserUpdateDto = userToUpdate
            };
            
            var handler = new UpdateUserCommandHandler
                (
                _moqUnitOfWork.Object,
                _moqUserWriteRepository.Object, 
                _moqUserReadRepository.Object
                );
            

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userToUpdate.Email, result!.Email);

            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _moqUserWriteRepository
                .Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Models.User>(), It.IsAny<CancellationToken>()), Times.Once);

            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task UpdateUserCommandHandler_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            _moqUserReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(),
                UserUpdateDto = UsersTestData.GetFakeUserUpdateDto()
            };

            var handler = new UpdateUserCommandHandler
                (
                _moqUnitOfWork.Object,
                _moqUserWriteRepository.Object, 
                _moqUserReadRepository.Object
                );

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));
            
            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            
            _moqUserWriteRepository
                .Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Models.User>(), It.IsAny<CancellationToken>()), Times.Never);
           
            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


    }
}
