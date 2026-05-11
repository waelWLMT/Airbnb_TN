using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Handlers;
using Application.UseCases.Queries;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Tests.Helpers;

namespace UnitTests.ApplicationTests.HandlerTests
{
    public class GetUserByIdQueryHandlerTests
    {
        
        private readonly Mock<IUserReadRepository> _moqUserReadRepository;

        public GetUserByIdQueryHandlerTests()
        {
         
            _moqUserReadRepository = new Mock<IUserReadRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = UsersTestData.GetSampleUser();

            _moqUserReadRepository
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            var query = new GetUserByIdQuery() { Id = userId };
            var handler = new GetUserByIdQueryHandler(_moqUserReadRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result!.Id);
            Assert.Equal(expectedUser.Email, result.Email);
        
            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _moqUserReadRepository
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

           

            var query = new GetUserByIdQuery() { Id = userId };
            var handler = new GetUserByIdQueryHandler(_moqUserReadRepository.Object);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            Assert.Null(result);
            
            _moqUserReadRepository
                .Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);

        }





    }
}
