using Application.DTOs.Input;
using Application.DTOs.Output;
using Application.Ports.Outbound;
using Application.UseCases.UserCases;
using Domain.Entities;
using Moq;
using Xunit;    

namespace Application.Test;

public class CreateUserTest
{
    [Fact]
    public async Task Should_Create_User()
    {
        //arrange
        var input = new CreateUserInput
        {
            Email = "test@hotmail.com",
            Password = "123456@Test",
            FullName = "Mock user",
        };

        var user = new User 
        {
            FullName = "Mock user",
            Email = "test@hotmail.com",
            Id = "123"
        };

        var mockUserIdentityService = new Mock<IUserIdentityService>();

        mockUserIdentityService.Setup(m => m.FindUserByEmail(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((User?)null);

        mockUserIdentityService.Setup(m => m.CreateUserAsync(input.Email, input.Password, input.FullName, CancellationToken.None)).ReturnsAsync(user);

        //act
        var createUser = new CreateUser(mockUserIdentityService.Object);
        var result = await createUser.Execute(input, CancellationToken.None);
        
        //assert
        Assert.Equal(input.Email, result.Email);
        Assert.Equal(input.FullName, result.FullName);
    }

    [Fact]
    public async Task Should_Thrown_Exception_If_Email_Is_Already_Registered()
    {
        
        //arrange
        var input = new CreateUserInput
        {
            Email = "test@hotmail.com",
            Password = "123456@Test",
            FullName = "Mock user",
        };
        
        var mockUserIdentityService = new Mock<IUserIdentityService>();
        mockUserIdentityService.Setup(m => m.FindUserByEmail(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new User());
        //act
        
        var createUser = new CreateUser(mockUserIdentityService.Object);
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            createUser.Execute(input, CancellationToken.None));
        
        //assert
        Assert.Equal("An user with this email already exists.", ex.Message);
        mockUserIdentityService.Verify(x => x.CreateUserAsync(input.Email, input.Password, input.FullName, CancellationToken.None), Times.Never);
    }
}