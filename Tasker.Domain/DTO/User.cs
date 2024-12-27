using Tasker.Domain.MasterData;

namespace Tasker.Domain.DTO;

public record User
{
    public int UserId { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Salt { get; set; }

    public  int Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public  int Status { get; set; }
}