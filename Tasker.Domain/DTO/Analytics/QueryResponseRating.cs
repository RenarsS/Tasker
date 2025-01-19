namespace Tasker.Domain.DTO.Analytics;

public class QueryResponseRating 
{
    public int QueryId { get; set; }

    public int ResponseId { get; set; }
    
    public float Rating { get; set; }
}