using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Validators;

namespace UnitTests.ApplicationTests.ValidatorTest
{
    public class UserCreateCommandValidatorTests
    {
        private readonly UserCreateCommandValidator _validator;

        public UserCreateCommandValidatorTests()
        {
            _validator = new UserCreateCommandValidator();
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenUserCreateDtoIsNull()
        {
            // Arrange
            var command = new Application.UseCases.Commands.CreateUserCommand
            {
                UserCreateDto = null
            };
            
            // Act
            var result = await Task.Run(() => { return _validator.Validate(command); });
            
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserCreateDto" && e.ErrorMessage == "Les données de création de l'utilisateur sont obligatoires.");
       
        }
    }
}
