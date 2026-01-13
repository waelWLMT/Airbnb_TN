using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Commands;
using Application.UseCases.Validators;

namespace Tests.ApplicationTests.ValidatorTest
{
    public class DeleteUserCommandValidatorTests
    {
        private readonly DeleteUserCommandValidator _validator;

        public DeleteUserCommandValidatorTests()
        {
            _validator = new DeleteUserCommandValidator();
        }

        [Fact]  
        public async Task Validate_ShouldReturnError_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteUserCommand
            {
                Id = Guid.Empty
            };

            // Act
            var result = await Task.Run(() => { return _validator.Validate(command); });
            
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id" && e.ErrorMessage == "User Id must be a valid GUID.");
        
        }

    }
}
