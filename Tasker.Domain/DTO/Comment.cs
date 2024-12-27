namespace Tasker.Domain.DTO;

public record Comment
{
    public int CommentId { get; set; }

    public int Task { get; set; }

    public  int User { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }
};