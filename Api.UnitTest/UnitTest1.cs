using System.Net.Mime;
using Api.Controllers;
using Application.DTOs.Input;
using Application.Ports.Outbound;
using Application.Ports.UsersCaseUse;
using Microsoft.AspNetCore.Mvc;

namespace Api.UnitTest;

using Moq;

public class UnitTest1
{
    [Fact]
    public async Task Execute_Should_Create_User_When_Valid_Input_Model()
    {
        //arrange
        var mockCreateUser = new Mock<ICreateUser>();
        
        var input = new CreateUserInput
        {
            Email = "test@hotmail.com",
            Password = "123456@Test",
            FullName = "Mock user",
        };
        
        var expectedOutput = new Application.DTOs.Output.CreateUserOutput
        {
            Id = Guid.NewGuid().ToString(),
            Email = input.Email,
            FullName = input.FullName
        };

        mockCreateUser
            .Setup(x => x.Execute(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedOutput);
        
        var controller = new UserController(mockCreateUser.Object);
        
        //act
        var result = await controller.CreateUser(input, CancellationToken.None);
        
        
        //assert
        Assert.IsType<OkObjectResult>(result);
        mockCreateUser.Verify(x => x.Execute(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Execute_Should_Return_BadRequest_When_Email()
    {
        //arrange
        
        var input = new CreateUserInput
        {
            Email = "test@hotmail.com",
            Password = "123456@Test",
            FullName = "Mock user",
        };
        
        var mockCreateUser = new Mock<ICreateUser>();
        
        mockCreateUser.Setup(x => x.Execute(input, It.IsAny<CancellationToken>())).Throws(new Exception("An user with this email already exists."));
        
        //act
        var controller = new UserController(mockCreateUser.Object);
        
        var result = await controller.CreateUser(input, CancellationToken.None);
        
        //assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}