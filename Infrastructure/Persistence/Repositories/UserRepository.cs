using Application.Ports.Outbound;
using Application.Ports.Outbound.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetById(int id)
    {
        User? user = await context.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        User? user = await context.Users.FirstAsync(u => u.Email.Equals(email));
        return user;
    }

    public async Task NewUser(User user,  CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public void UpdateUser(User user)
    {
        context.Users.Update(user);
    }
}