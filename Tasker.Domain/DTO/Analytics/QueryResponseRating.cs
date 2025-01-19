namespace Tasker.Domain.DTO.Analytics;

public class QueryResponseRating : Rating
{
    public int QueryId { get; set; }

    public int ResponseId { get; set; }
    
    public float Rating { get; set; }
}