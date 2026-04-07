using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure
{
    /// <summary>
    /// The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly PropertyBookingDbContext _context;
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public UnitOfWork(PropertyBookingDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get service by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>An object?</returns>
        public object GetServiceByType(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {serviceType.Name} not found.");
            }

            return service;
        }

        /// <summary>
        /// Roll the back asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task</returns>
        public async Task RollBackAsync(CancellationToken cancellationToken)
        {
            await _context.DisposeAsync();
        }
    }
}
