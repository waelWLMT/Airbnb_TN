using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Utils;
using Infrastructure;
using Infrastructure.Repositories;
using Moq;
using Tests.Helpers;

namespace Tests.InfrastructureTests
{
    public class UserReadRepositoryTest
    {
        private readonly UsersDbContext _dbContext;

        public UserReadRepositoryTest()
        {
            _dbContext = Helpers.DbHelper.GetInMemoryUsersDbContext();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);

            // Act
            var result = await userReadRepository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsers_WhenUsersExist()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);

            var users = UsersTestData.GetSamplesUsers();


            _dbContext.Users.AddRange(users);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await userReadRepository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(users.Count, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);
            var users = UsersTestData.GetSamplesUsers();

            _dbContext.Users.AddRange(users);
            await _dbContext.SaveChangesAsync();

            var userId = users[0].Id;

            // Act
            var result = await userReadRepository.GetByIdAsync(userId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);           
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);

            // Act
            var result = await userReadRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async Task GetOneAsync_ReturnsUserWithRole_WhenUserExists()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);
            var users = UsersTestData.GetSamplesUsers();
            _dbContext.Users.AddRange(users);
            await _dbContext.SaveChangesAsync();
            var userEmail = users[0].Email;

            // Act
            var result = await userReadRepository.GetOneAsync(CancellationToken.None, new FindOptions
            {
                Predicate = u => u.Email == userEmail,
                Includes = new System.Linq.Expressions.Expression<Func<Domain.Models.User, object>>[] { x => x.UserRole },
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("xunit.test@gmail.com", result.Email);
            Assert.NotNull(result.UserRole);
        }

        [Fact]
        public async Task GetOneAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);

            // act
            var result = await userReadRepository.GetOneAsync(It.IsAny<CancellationToken>(), It.IsAny<FindOptions>());

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async Task GetAllUsers_ReturnAllUsersWithRole_WhenUsersExists()
        {
            // Arrange
            var userReadRepository = new UserReadRepository(_dbContext);
            var users = UsersTestData.GetSamplesUsers();
            _dbContext.Users.AddRange(users);
            await _dbContext.SaveChangesAsync();
           
            
            // Act
            var result = await userReadRepository.GetAllAsync(CancellationToken.None, new FindOptions
            {
                Includes = new Expression<Func<User, object>>[] { x => x.UserRole },
            });

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, user => Assert.NotNull(user.UserRole));

        }

    }
}
