using Application.DTOs.Input;
using Application.DTOs.Output;
using Application.Ports.Inbound;
using Application.Ports.Outbound;
using Application.Ports.Outbound.Repositories;
using Domain.Entities;

namespace Application.UseCases.UserCases;

public class CreateUser(
    IUserIdentityService userIdentityService,
    IUnitOfWork unitOfWork) : ICreateUser
{
    public async Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken)
    {
        var userLogin = await userIdentityService.FindUserByEmail(input.Email, cancellationToken);
        if (userLogin is not null)
            throw new Exception("An user with this email already exists.");

        User user = input.ConvertToUser();
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            await unitOfWork.userRepository.NewUser(user, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            await userIdentityService.CreateUserAsync(input.Email, input.Password, input.FullName,
                cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateUserOutput
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new Exception("An error occured while creating the user.", e);
        }
    }
}