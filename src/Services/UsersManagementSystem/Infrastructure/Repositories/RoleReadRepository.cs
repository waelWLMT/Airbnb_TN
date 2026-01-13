using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleReadRepository : IRoleReadRepository
    {
        private readonly UsersDbContext _context;

        public RoleReadRepository(UsersDbContext usersDbContext)
        {
            _context = usersDbContext;
        }
        public async Task<List<Role>?> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }
    }
}
