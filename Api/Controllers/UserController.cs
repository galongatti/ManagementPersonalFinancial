using Application.DTOs.Input;
using Application.DTOs.Output;
using Application.Ports.UsersCaseUse;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(ICreateUser createUser) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserInput input, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
            throw new Exception("Invalid input");
        
        try
        {
            CreateUserOutput output = await createUser.Execute(input, cancellationToken);
            return Ok(output);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
}