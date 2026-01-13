using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class UserWriteRepository : IUserWriteRepository
    {
        protected readonly UsersDbContext _context;    
        public UserWriteRepository(UsersDbContext context)
        {
            _context = context;            
        }
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
        }
        public async Task DeleteAsync(User user, CancellationToken ct = default)
        {
            await Task.Run(() => _context.Users.Remove(user), ct);
        }       
        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            await Task.Run(() => _context.Users.Update(user), ct);
        }
    }
}
