using System.ComponentModel.DataAnnotations;

namespace CusstomerManagement.Application.DTOs;

public class UpdateCustomerDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public byte[] RowVersion { get; set; } = null!;
}
