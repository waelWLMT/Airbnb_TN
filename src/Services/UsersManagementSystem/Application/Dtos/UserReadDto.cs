using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public RoleReadDto? UserRoleReadDto { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
