using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
