using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Models;

namespace Application.Services
{
    public static class UserRoleBuilderService
    {
        public static RoleReadDto BuildUserRoleReadDto(Role userRole)
        {
            var userRoleReadDto = new RoleReadDto
            {
                Id = userRole.Id,
                Code = userRole.Code,
                Libelle = userRole.Libelle
            };
            return userRoleReadDto;

        }

        public static List<RoleReadDto> BuildUserRoleReadDtoList(List<Role> roles)
        {
            var roleReadDtos = new List<RoleReadDto>();
            foreach (var role in roles)
            {
                var userRoleReadDto = BuildUserRoleReadDto(role);
                roleReadDtos.Add(userRoleReadDto);
            }
            return roleReadDtos;
        }

    }
}
