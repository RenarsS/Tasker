using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Domain;

public class TimeReport
{
    public int TimeReportId { get; set; }

    public required Task Task { get; set; }

    public required User User { get; set; }

    public DateTime Beggining { get; set; }

    public DateTime End { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public DateOnly ReportDate { get; set; }
}