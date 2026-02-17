using Application.Ports.Outbound;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Service;

public class UserIdentityService(UserManager<User> userManager) : IUserIdentityService
{
    public async Task<User> CreateUserAsync(string email, string password, string fullName,
        CancellationToken cancellationToken)
    {
        var user = new User { UserName = email, Email = email, FullName = fullName };
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded ? user : throw new Exception("An error occured while trying to create the user");
    }

    public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await userManager.FindByEmailAsync(email);
    }
}