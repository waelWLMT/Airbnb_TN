using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Repositories;
using Moq;
using Tests.Helpers;

namespace Tests.ApplicationTests.HandlerTests
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly Mock<IUserReadRepository> _moqReadRepository;
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;

        public GetAllUsersQueryHandlerTests()
        {
            _moqReadRepository = new Mock<IUserReadRepository>();
            _moqUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUsers = UsersTestData.GetSamplesUsers();

            _moqReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()))
                .ReturnsAsync(expectedUsers);

            _moqUnitOfWork
                .Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqReadRepository.Object);

            var handler = new Application.UseCases.Handlers.GetAllUsersQueryHandler(_moqUnitOfWork.Object);

            var query = new Application.UseCases.Queries.GetAllUsersQuery
            {
                IsReadOnly = true,
                UserWithRole = true
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count);
            Assert.Equal(expectedUsers, result);

            _moqReadRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _moqReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()))
                .ReturnsAsync(new List<User>());

            _moqUnitOfWork
                .Setup(uow => uow.GetRequiredRepository<IUserReadRepository>())
                .Returns(_moqReadRepository.Object);

            var handler = new Application.UseCases.Handlers.GetAllUsersQueryHandler(_moqUnitOfWork.Object);

            var query = new Application.UseCases.Queries.GetAllUsersQuery
            {
                IsReadOnly = true,
                UserWithRole = false
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result!);

            _moqReadRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions?>()), Times.Once);
        }
    }
}
