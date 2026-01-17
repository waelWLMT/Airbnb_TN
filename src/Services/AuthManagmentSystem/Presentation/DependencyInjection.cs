using Application.Interfaces;
using Application.Services;
using Infrastructure.Clients;
using Providers;

namespace Presentation
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IAuthOrchestrator, AuthOrchestrator>();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();            
            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            // Register provider services here
            services.AddScoped<IAuthenticatorService, JwtAuthService>();
            services.AddScoped<IAuthenticatorService, FacebookAuthService>();
            services.AddScoped<IAuthenticatorService, GoogleAuthService>();

            return services;
        }

        public static IServiceCollection AddInfrastructre(this IServiceCollection services, string userServiceUrl)
        {
            services.AddHttpClient<IUserManagementClient, UserManagementClient>(client =>
            {
                client.BaseAddress = new Uri(userServiceUrl);
            });

            return services;
        }

       

        public static IServiceCollection AddAll(this IServiceCollection services, string userServiceUrl)
        {
            services.AddPresentation();
            services.AddInfrastructre(userServiceUrl);
            services.AddProviders();
            services.AddApplication();
            
            return services;
        }
    }
    }
