using Application.Ports.Outbound;
using Application.Ports.Outbound.Repositories;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{ 
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserIdentityService, UserIdentityService>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
}