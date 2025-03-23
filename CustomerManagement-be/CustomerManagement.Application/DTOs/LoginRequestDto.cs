using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Application.DTOs;

public class LoginRequestDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
