using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectApplication(this IServiceCollection services)
        {
            // Inject application Serices here
            return services;
        }
    }
}
