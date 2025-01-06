namespace Tasker.Domain.DTO;

public class Task
{
    public int TaskId { get; set; }

    public int TaskType { get; set; }

    public required string Title { get; set; }
    
    public required string Description { get; set; }

    public required int Status { get; set; }

    public required int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime Due { get; set; }

    public string? VectorId { get; set; }
};