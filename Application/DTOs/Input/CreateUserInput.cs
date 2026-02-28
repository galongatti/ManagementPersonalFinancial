using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Input;

public class CreateUserInput
{
    [Required] public required string FullName { get; set; }

    [Required] [EmailAddress] public required string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{6,}$",
        ErrorMessage =
            "A senha deve conter pelo menos 6 caracteres, incluindo pelo menos uma letra maiúscula, uma minúscula, um número e um caractere não alfanumérico.")]
    public required string Password { get; set; }


    public User ConvertToUser()
    {
        return new User
        {
            Email = Email,
            FullName = FullName,
            IsActive = true
        };
    }
}