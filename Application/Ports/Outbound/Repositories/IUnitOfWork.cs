using Application.Ports.Outbound.Repositories;

namespace Application.Ports.Outbound;

public interface IUnitOfWork
{
    IUserRepository userRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}