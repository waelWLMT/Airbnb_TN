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
       

        public GetAllUsersQueryHandlerTests()
        {
            _moqReadRepository = new Mock<IUserReadRepository>();
           
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUsers = UsersTestData.GetSamplesUsers();

            _moqReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()))
                .ReturnsAsync(expectedUsers);

          

            var handler = new Application.UseCases.Handlers.GetAllUsersQueryHandler(_moqReadRepository.Object);

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
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _moqReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()))
                .ReturnsAsync(new List<User>());
          

            var handler = new Application.UseCases.Handlers.GetAllUsersQueryHandler(_moqReadRepository.Object);

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
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()), Times.Once);
        }
    }
}
