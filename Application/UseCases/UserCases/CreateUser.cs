using Application.DTOs.Input;
using Application.DTOs.Output;
using Application.Ports.Outbound;
using Application.Ports.UsersCaseUse;
using Domain.Entities;

namespace Application.UseCases.UserCases;

public class CreateUser(IUserIdentityService userIdentityService) : ICreateUser
{
    public async Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken)
    {
        User? userAlreadyExists = await userIdentityService.FindUserByEmail(input.Email, cancellationToken);

        if (userAlreadyExists is not null)
            throw new Exception("An user with this email already exists.");

        User newUser =
            await userIdentityService.CreateUserAsync(input.Email, input.Password, input.FullName, cancellationToken);

        return new CreateUserOutput
        {
            Id = newUser.Id,
            Email = newUser.Email,
            FullName = newUser.FullName
        };
    }
}