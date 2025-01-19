namespace Tasker.Domain.Events;

public record NewTaskCreated
{
    public int[] RelevantTaskCount { get; init; }
    public Tasker.Domain.DTO.Task? Task { get; init; }
};