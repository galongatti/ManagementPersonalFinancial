using Application.DTOs.Input;
using Application.DTOs.Output;

namespace Application.Ports.Inbound;

public interface ICreateUser
{
    Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken);
}