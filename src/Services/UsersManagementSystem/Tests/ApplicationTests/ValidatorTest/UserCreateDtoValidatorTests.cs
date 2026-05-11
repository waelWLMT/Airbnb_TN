using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Validators;

namespace UnitTests.ApplicationTests.ValidatorTest
{
    public class UserCreateDtoValidatorTests
    {
        private readonly UserCreateDtoValidator _validator;

        public UserCreateDtoValidatorTests()
        {
            _validator = new UserCreateDtoValidator();
        }

        [Fact]
        public async Task Validate_ShouldReturnErrors_WhenPropertiesAreInvalid()
        {
            // Arrange
            var dto = new Application.Dtos.UserCreateDto
            {
                Nom = "",
                Prenom = "",
                Email = "invalidemail",
                Password = "",
                RoleId = 0
            };

            // Act
            var result = await Task.Run(() => { return _validator.Validate(dto);});
           
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Nom" && e.ErrorMessage == "Le nom est obligatoire.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Prenom" && e.ErrorMessage == "Le prénom est obligatoire.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "L'email n'est pas valide.");

            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe doit contenir au moins 6 caractères.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe doit contenir au moins une lettre majuscule.");         
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe doit contenir au moins une lettre minuscule.");            
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe doit contenir au moins un chiffre.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe doit contenir au moins un caractère spécial.");
            Assert.Contains(result.Errors, e => e.PropertyName == "RoleId" && e.ErrorMessage == "Le rôle est obligatoire et doit être valide.");
        
        }

    }
}
