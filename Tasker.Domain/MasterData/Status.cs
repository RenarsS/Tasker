namespace Tasker.Domain.MasterData;

public record Status
{
    public int StatusId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}