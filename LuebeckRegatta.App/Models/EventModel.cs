namespace LuebeckRegatta.App.Models;

/// <summary>
/// Represents a regatta event
/// </summary>
public class EventModel
{
    public int? Lanes { get; set; }
    public int? ID { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Title { get; set; }
}
