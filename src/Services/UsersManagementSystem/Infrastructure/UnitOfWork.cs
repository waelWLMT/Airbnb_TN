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
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(UsersDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public T GetRequiredRepository<T>()
        {     
            return (T) _serviceProvider.GetRequiredService<T>() 
                ?? throw new InvalidOperationException($"Repository {nameof(T)} not found");
        }

        public async Task RollBackAsync(CancellationToken ct = default)
        {
            await _context.DisposeAsync();
        }
    }
}
