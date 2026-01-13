using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Services;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Helpers;
using WebApi.Controllers;

namespace Tests.PresentationTests
{

    public class UserControllerTest
    {
        private readonly UserController _userController;
        private readonly Mock<IMediator> _mediator;

        public UserControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _userController = new UserController(_mediator.Object);
        }

        
        [Fact]
        public async Task GetAllUsers_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<User>();

            _mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(users);
            // Act
            var result = await _userController.GetAllUsers(false, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsListOfUserReadDto_WhenUsersExist()
        {
            // Arrange
            var users = UsersTestData.GetSamplesUsers();

            _mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(users);
            // Act
            var result = await _userController.GetAllUsers(false, true);            

            // Assert            
            Assert.NotNull(result);
            Assert.NotEmpty(result);  
           
        }

        [Fact]
        public async Task GetUserById_ReturnNull_WhenNoUserExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userController.GetUserById(userId, true, true);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ShouldCreateAndReturnsUserReadDto()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UsersTestData.GetSampleUser());
            // Act
            var result = await _userController.CreateUser(UsersTestData.GetFakeUserCreateDto());
            var resultType = result?.GetType();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(UserReadDto), resultType);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userController.DeleteUser(userId);

            // Assert
            Assert.IsAssignableFrom<IActionResult>(result);

        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateAndReturnUserReadDto()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UsersTestData.GetSampleUser());
            
            var userUpdateDto = new UserUpdateDto
            {

                RoleId = 1,
                Nom = "Doe",
                Prenom = "John",
                Email = "xunit.test@gmail.com",
                Password = "test"
            };

            // Act
            var result = await _userController.UpdateUser(userUpdateDto, Guid.NewGuid());
            var resultType = result?.GetType();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(UserReadDto), resultType);
            Assert.Equal(1, result.RoleId);
            Assert.Equal("Doe", result.Nom);
            Assert.Equal("John", result.Prenom);
            Assert.Equal("xunit.test@gmail.com", result.Email);

        }

        [Fact]
        public async Task GetUserByEmailAndPassword_ShouldReturnUserReadDto_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "xunit.test@gmail.com";
            var password = "testpassword";

            _mediator.Setup(m => m.Send(It.IsAny<GetUserByEmailAndPasswordQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UsersTestData.GetSampleUser());

            // Act
            var result = await _userController.GetUserByEmailAndPassword(email, password);
            var resultType = result?.GetType();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(UserReadDto), resultType);
            Assert.Equal("xunit.test@gmail.com", result.Email);
        
        }

        
    }

}
