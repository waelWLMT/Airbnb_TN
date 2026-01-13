using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Tests.Helpers
{
    public static class DbHelper
    {
        public static UsersDbContext GetInMemoryUsersDbContext()
        {
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new UsersDbContext(options);

            return dbContext;
        }
    }
}
