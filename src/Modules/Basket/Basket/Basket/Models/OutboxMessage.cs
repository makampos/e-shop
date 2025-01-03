namespace Basket.Basket.Models;

public class OutboxMessage : Entity<Guid>
{
    public string Type { get; set; } = default!;
    public string Content { get; set; } = default!; // JSON payload
    public DateTime OccuredOn { get; set; } = default!;
    public DateTime? ProcessOn { get; set; } = default!;
}