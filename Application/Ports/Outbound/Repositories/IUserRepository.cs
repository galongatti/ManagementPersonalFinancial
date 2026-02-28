using Domain.Entities;

namespace Application.Ports.Outbound.Repositories;

public interface IUserRepository
{
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    Task NewUser(User user, CancellationToken cancellationToken);
    void UpdateUser(User user);
}