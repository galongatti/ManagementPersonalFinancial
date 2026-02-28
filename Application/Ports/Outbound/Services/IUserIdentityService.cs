using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Ports.Outbound;

public interface IUserIdentityService
{
    Task<IdentityUser> CreateUserAsync(string username, string password, string fullName, CancellationToken cancellationToken);
    
    Task<IdentityUser?> FindUserByEmail(string email, CancellationToken cancellationToken);
}