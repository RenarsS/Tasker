using Tasker.Domain.DTO;

namespace Tasker.Domain;

public record AuditLog
{
    public int LogId { get; set; }

    public required User User { get; set; }

    public required string Action { get; set; }

    public DateTime Time { get; set; }

    public string? Details { get; set; }
};