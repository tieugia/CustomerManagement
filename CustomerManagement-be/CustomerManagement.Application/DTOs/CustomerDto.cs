namespace CudstomerManagement.Application.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
