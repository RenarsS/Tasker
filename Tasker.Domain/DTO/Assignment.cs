using Tasker.Domain.MasterData;

namespace Tasker.Domain.DTO;

public class Assignment
{
    public int AssignmentId { get; set; }

    public int Task { get; set; }

    public  int User { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }
    
    public int Status { get; set; }
}