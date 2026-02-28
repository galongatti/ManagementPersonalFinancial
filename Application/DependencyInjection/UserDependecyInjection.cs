using Application.Ports.Inbound;
using Application.UseCases.UserCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class UserDependecyInjection
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateUser, CreateUser>();

        return services;
    }
}