using Domain.Entities;

namespace Application.Ports.Outbound;

public interface IUserIdentityService
{
    Task<User> CreateUserAsync(string username, string password, string fullName, CancellationToken cancellationToken);
    
    Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken);
}