using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Models;

namespace Application.Services
{
    public static class UserBuilderService
    {
        public static UserReadDto BuildUserReadDto(User user)
        {
            var userReadDto = new UserReadDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                RoleId = user.RoleId,
                UserRoleReadDto = user.UserRole != null ? UserRoleBuilderService.BuildUserRoleReadDto(user.UserRole) : null,
                CreatedAt = user.CreatedAt
            };

            return userReadDto;
        }
        public static List<UserReadDto> BuildUserReadDtoList(List<User> users)
        {
            var userReadDtoList = new List<UserReadDto>();
            foreach (var user in users)
            {
                var userReadDto = BuildUserReadDto(user);
                userReadDtoList.Add(userReadDto);
            }
            return userReadDtoList;
        }

        public static User BuildUser(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Nom = userCreateDto.Nom,
                Prenom = userCreateDto.Prenom,
                Email = userCreateDto.Email,
                RoleId = userCreateDto.RoleId,
                PasswordHash = PasswordService.HashPassword(userCreateDto.Password),
                CreatedAt = DateTime.UtcNow
            };
            return user;
        }        
    }
}
