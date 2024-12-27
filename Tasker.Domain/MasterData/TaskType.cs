namespace Tasker.Domain.MasterData;

public record TaskType
{
    public int TaskTypeId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}