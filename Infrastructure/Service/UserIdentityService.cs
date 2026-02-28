using Application.Ports.Outbound;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Service;

public class UserIdentityService(UserManager<IdentityUser> userManager) : IUserIdentityService
{
    public async Task<IdentityUser> CreateUserAsync(string email, string password, string fullName,
        CancellationToken cancellationToken)
    {
        IdentityUser identityUser = new() { UserName = email, Email = email };
        IdentityResult result = await userManager.CreateAsync(identityUser, password);
        return result.Succeeded
            ? identityUser
            : throw new Exception("An error occured while trying to create the user");
    }

    public async Task<IdentityUser?> FindUserByEmail(string email, CancellationToken cancellationToken)
    {
        var identityUser = await userManager.FindByEmailAsync(email);
        return identityUser;
    }
}