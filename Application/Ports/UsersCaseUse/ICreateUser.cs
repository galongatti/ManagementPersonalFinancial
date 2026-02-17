using Application.DTOs.Input;
using Application.DTOs.Output;

namespace Application.Ports.UsersCaseUse;

public interface ICreateUser
{
    Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken);
}