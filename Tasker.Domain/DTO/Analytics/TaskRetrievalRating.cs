namespace Tasker.Domain.DTO.Analytics;

public class TaskRetrievalRating
{
    public int TaskId { get; set; }

    public int RetrievalTaskId { get; set; }

    public float Rating { get; set; }
}