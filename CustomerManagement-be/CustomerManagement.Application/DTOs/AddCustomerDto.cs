using System.ComponentModel.DataAnnotations;

namespace CusstomerManagement.Application.DTOs;

public class AddCustomerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
}
