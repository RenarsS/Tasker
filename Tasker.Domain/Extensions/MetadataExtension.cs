using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Domain.Extensions;

public static class MetadataExtension
{
    public static Dictionary<string, object> GetMetadata(this Task task)
    {
        var metadata = new Dictionary<string, object>();
        metadata.Add(MetadataProperties.TaskId, task.TaskId);
        return metadata;
    }
    
    public static Dictionary<string, object> GetMetadata(this Assignment assignment)
    {
        var metadata = new Dictionary<string, object>();
        metadata.Add(MetadataProperties.TaskId, assignment.Task);
        metadata.Add(MetadataProperties.AssignmentId, assignment.AssignmentId);
        return metadata;
    }
    
    public static Dictionary<string, object> GetMetadata(this Comment comment)
    {
        var metadata = new Dictionary<string, object>();
        metadata.Add(MetadataProperties.TaskId, comment.Task);
        metadata.Add(MetadataProperties.CommentId, comment.CommentId);
        return metadata;
    }

    public static Dictionary<string, object> GetMetadata(this Query query)
    {
        var metadata = new Dictionary<string, object>();
        metadata.Add(MetadataProperties.QueryId, query.QueryId);
        metadata.Add(MetadataProperties.ResponseId, query.ResponseId);
        return metadata;
    }
}