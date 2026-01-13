using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.Handlers;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Moq;
using Tests.Helpers;

namespace Tests.ApplicationTests.HandlerTests
{
    public class GetUserByEmailAndPasswordQueryHandlerTests
    {
        private readonly Mock<IUserReadRepository> _moqUserReadRepository;
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;

        public GetUserByEmailAndPasswordQueryHandlerTests()
        {
            _moqUserReadRepository = new Mock<IUserReadRepository>();
            _moqUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenEmailAndPasswordAreValid()
        {
            // Arrange
            var query = new GetUserByEmailAndPasswordQuery
            {
                Email = "xunit.test@gmail.com",
                Password = "test"
            };

            _moqUserReadRepository
                .Setup(repo => repo.GetOneAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()))
                .ReturnsAsync(UsersTestData.GetSampleUser());

            _moqUnitOfWork.Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqUserReadRepository.Object);

            // Act
            var handler = new GetUserByEmailAndPasswordQueryHandler(_moqUnitOfWork.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(query.Email, result!.Email);
            Assert.Equal(query.Password, result.PasswordHash);

            _moqUnitOfWork
                .Verify(uow => uow.GetRequiredRepository<IUserReadRepository>(), Times.Once);

            _moqUserReadRepository
                .Verify(repo => repo.GetOneAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEmailOrPasswordAreInvalid()
        {
            // Arrange
            var query = new GetUserByEmailAndPasswordQuery
            {
                Email = "",
                Password = "wrong_password"
            };

            _moqUserReadRepository
                .Setup(repo => repo.GetOneAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()))
                .ReturnsAsync((User?)null);

            _moqUnitOfWork
                .Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqUserReadRepository.Object);

            
            var handler = new GetUserByEmailAndPasswordQueryHandler(_moqUnitOfWork.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _moqUnitOfWork
                .Verify(uow => uow.GetRequiredRepository<IUserReadRepository>(), Times.Once);
            
            _moqUserReadRepository
                .Verify(repo => repo.GetOneAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()), Times.Once);

        }
    }


}
