namespace Tasker.Domain.DTO;

public class Response
{
    public int ResponseId { get; set; }

    public int CommentId { get; set; }

    public string Content { get; set; }

    public int InputTokenCount { get; set; }

    public int OutputTokenCount { get; set; }

    public int TotalTokenCount { get; set; }

    public int UserId { get; set; }
}