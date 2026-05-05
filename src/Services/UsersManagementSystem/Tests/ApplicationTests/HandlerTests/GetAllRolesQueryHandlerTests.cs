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
        

        public GetAllRolesQueryHandlerTests()
        {
            _moqUserReadRepository = new Mock<IRoleReadRepository>();
           
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfRoles_WhenRolesExist()
        {
            // Arrange
            var expectedRoles = RolesTestData.GetSampleRoles();
                        
            _moqUserReadRepository
                .Setup(repo => repo.ListAsync(It.IsAny<CancellationToken>(), false))
                .ReturnsAsync(expectedRoles);

            
            
            var handler = new Application.UseCases.Handlers.GetAllRolesQueryHandler(_moqUserReadRepository.Object);
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
