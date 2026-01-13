using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.UseCases.Commands;
using Application.UseCases.Handlers;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Tests.Helpers;

namespace Tests.ApplicationTests.HandlerTests
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;
        private readonly Mock<IUserWriteRepository> _moqUserWriteRepository;        

        public CreateUserCommandHandlerTests()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>();
            _moqUserWriteRepository = new Mock<IUserWriteRepository>();
            
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenValidRequest()
        {
            // Arrange
            var userCreateDto = UsersTestData.GetFakeUserCreateDto();

            _moqUserWriteRepository
                .Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserWriteRepository>())
                .Returns(_moqUserWriteRepository.Object);

            _moqUnitOfWork
                .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateUserCommand() { UserCreateDto = userCreateDto };
            var handler = new CreateUserCommandHandler(_moqUnitOfWork.Object);

            // Act  
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userCreateDto.Email, result.Email);

            _moqUserWriteRepository
                .Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenNotValidRequest()
        {
            // Arrange
            UserCreateDto userCreateDto = null!;            

            _moqUserWriteRepository
                .Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _moqUnitOfWork 
                .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserWriteRepository>())
                .Returns(_moqUserWriteRepository.Object);

            var command = new CreateUserCommand() { UserCreateDto = userCreateDto };
            var handler = new CreateUserCommandHandler(_moqUnitOfWork.Object);
           
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);           
            
            _moqUserWriteRepository
                .Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
            
            _moqUnitOfWork
                .Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
