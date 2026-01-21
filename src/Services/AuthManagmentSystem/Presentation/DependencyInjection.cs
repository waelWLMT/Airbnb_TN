using Application.AuthProviders;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Clients;
namespace Presentation
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthTokenService, AuthTokenService>();

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
            services.AddScoped<IAuthenticatorService, JwtAuthProvider>();
            services.AddScoped<IAuthenticatorService, FacebookAuthProvider>();
            services.AddScoped<IAuthenticatorService, GoogleAuthProvider>();

            return services;
        }

        public static IServiceCollection AddInfrastructre(this IServiceCollection services, string userServiceUrl)
        {
            services.AddHttpClient<IUserManagementHttpClient, UserManagementHttpClient>(client =>
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
