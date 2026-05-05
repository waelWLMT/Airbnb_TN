using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext _context;
        public UnitOfWork(UsersDbContext context)
        {
            _context = context;
        }
        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
        public async Task RollBackAsync(CancellationToken ct = default)
        {
            await _context.DisposeAsync();
        }
    }
}
