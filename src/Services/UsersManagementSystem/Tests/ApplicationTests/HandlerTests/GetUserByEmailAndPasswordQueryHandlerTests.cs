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
      

        public GetUserByEmailAndPasswordQueryHandlerTests()
        {
            _moqUserReadRepository = new Mock<IUserReadRepository>();
           
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

            var users = new List<User>() { UsersTestData.GetSampleUser() };            
            users[0].PasswordHash = PasswordService.HashPassword(query.Password);

            _moqUserReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()))
                .ReturnsAsync(users);

            // Act
            var handler = new GetUserByEmailAndPasswordQueryHandler(_moqUserReadRepository.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(query.Email, result!.Email);           

           
            _moqUserReadRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()), Times.Once);

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

            var users = new List<User>();

            _moqUserReadRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()))
                .ReturnsAsync(users);
            
            var handler = new GetUserByEmailAndPasswordQueryHandler(_moqUserReadRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
          
            _moqUserReadRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions<User>?>()), Times.Once);

        }
    }


}
