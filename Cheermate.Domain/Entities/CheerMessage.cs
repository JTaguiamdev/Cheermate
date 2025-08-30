namespace Cheermate.Domain.Entities;

public class CheerMessage
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}