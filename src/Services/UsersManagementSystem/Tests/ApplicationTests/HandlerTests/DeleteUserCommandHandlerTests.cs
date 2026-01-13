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

namespace Tests.ApplicationTests.HandlerTests
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;
        private readonly Mock<IUserWriteRepository> _moqUserWriteRepository;
        private readonly Mock<IUserReadRepository> _moqUserReadRepository;

        public DeleteUserCommandHandlerTests()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>();
            _moqUserWriteRepository = new Mock<IUserWriteRepository>();
            _moqUserReadRepository = new Mock<IUserReadRepository>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenValidRequest()
        {
            // Arrange
            var user = UsersTestData.GetSampleUser();

            _moqUserReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _moqUserWriteRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserWriteRepository>())
                .Returns(_moqUserWriteRepository.Object);

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqUserReadRepository.Object);

            _moqUnitOfWork
                .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteUserCommand() { Id = user.Id };
            var handler = new DeleteUserCommandHandler(_moqUnitOfWork.Object);

            // Act  
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);

            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _moqUserWriteRepository
                .Verify(repo => repo.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange    
            _moqUserReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);            

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserWriteRepository>())
                .Returns(_moqUserWriteRepository.Object);

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqUserReadRepository.Object);

            var command = new DeleteUserCommand() { Id = Guid.NewGuid() };
            var handler = new DeleteUserCommandHandler(_moqUnitOfWork.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));

            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _moqUserWriteRepository
                .Verify(repo => repo.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);

            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);


        }
    }
}
