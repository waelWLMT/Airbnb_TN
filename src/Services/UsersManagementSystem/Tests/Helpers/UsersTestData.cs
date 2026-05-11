using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Models;

namespace UnitTests.Helpers
{
    public static class UsersTestData
    {
        public static UserCreateDto GetFakeUserCreateDto()
        {
            return new UserCreateDto
            {
                RoleId = 1,
                Nom = "Doe",
                Prenom = "John",
                Email = "xunit.test@gmail.com",
                Password = "test"
            };
        }
        public static User GetSampleUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                RoleId = 1,
                Nom = "Doe",
                Prenom = "John",
                Email = "xunit.test@gmail.com",
                PasswordHash = "test"
            };
        }
        public static List<User> GetSamplesUsers()
        {
            return new List<User>() {
                new User {
                    Id = Guid.NewGuid(),
                    RoleId = 1,
                    Nom = "Doe",
                    Prenom = "John",
                    Email = "xunit.test@gmail.com",
                    PasswordHash = "test",
                    UserRole =  new Role { Id =1, Libelle="Admin", Code = "Admin"}
                },
                new User {
                    Id = Guid.NewGuid(),
                    RoleId = 2,
                    Nom = "Smith",
                    Prenom = "Jane",
                    Email = "",
                    PasswordHash = "test",
                    UserRole =  new Role { Id =2, Libelle="Voyageur", Code = "Voyageur"}
                }
                };

        }


        public static UserUpdateDto GetFakeUserUpdateDto()
        {
            return new UserUpdateDto
            {
                RoleId = 1,
                Nom = "Doe",
                Prenom = "John",
                Email = "xunit.test@gmail.com",
                Password = "test"
            };
        }
    }
}
