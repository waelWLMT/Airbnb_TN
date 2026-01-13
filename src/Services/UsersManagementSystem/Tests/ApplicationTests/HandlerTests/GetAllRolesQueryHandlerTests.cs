using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Tests.Helpers;

namespace Tests.ApplicationTests.HandlerTests
{
    public class GetAllRolesQueryHandlerTests
    {
        private readonly Mock<IRoleReadRepository> _moqUserReadRepository;
        private readonly Mock<IUnitOfWork> _moqUnitOfWork;

        public GetAllRolesQueryHandlerTests()
        {
            _moqUserReadRepository = new Mock<IRoleReadRepository>();
            _moqUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfRoles_WhenRolesExist()
        {
            // Arrange
            var expectedRoles = RolesTestData.GetSampleRoles();

            _moqUserReadRepository
                .Setup(repo => repo.GetAllRolesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRoles);

            _moqUnitOfWork
                .Setup(uow => uow.GetRequiredRepository<IRoleReadRepository>())
                .Returns(_moqUserReadRepository.Object);
            
            var handler = new Application.UseCases.Handlers.GetAllRolesQueryHandler(_moqUnitOfWork.Object);
            var query = new Application.UseCases.Queries.GetAllRolesQuery();
           
            // Act
            var result = await handler.Handle(query, CancellationToken.None);           
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result!.Count);
            Assert.Equal(expectedRoles, result);
        }


    }
}
