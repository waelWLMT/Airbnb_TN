using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.UseCases.Queries;
using MediatR;
using Moq;
using Tests.Helpers;
using WebApi.Controllers;

namespace Tests.PresentationTests
{
    public class RoleControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly RoleController _roleController;


        public RoleControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _roleController = new RoleController(_mediator.Object);
        }

        [Fact]
        public async Task GetAllRoles_ReturnsNotEmptyList_WhenRolesExist()
        {
            // Arrange
            var roles = RolesTestData.GetSampleRoles();

            _mediator.Setup(m => m.Send(It.IsAny<GetAllRolesQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(roles);

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(roles.Count, result.Count);
            Assert.IsType<List<RoleReadDto>?>(result);

            _mediator
                .Verify(m => m.Send(It.IsAny<GetAllRolesQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
