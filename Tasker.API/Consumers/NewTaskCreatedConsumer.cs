using MassTransit;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.Events;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Consumers;

public class NewTaskCreatedConsumer(IRecommendationService recommendationService, ICommentService commentService, IResponseService responseService) : IConsumer<NewTaskCreated>
{
    public async Task Consume(ConsumeContext<NewTaskCreated> context)
    {
        var message = context.Message;
        var task = message.Task;
        var limits = message.RelevantTaskCount;
        if (task is null)
        {
            return;
        }

        foreach (var limit in limits)
        {
            var recommendationResponse = await recommendationService.GenerateRecommendationResponse(task, limit);
            var recommendationComment = new Comment
            {
                Task = task.TaskId,
                Content = recommendationResponse.Content,
                User = recommendationResponse.UserId,
            };
            
            var comment = await commentService.CreateComment(recommendationComment);
            await responseService.LinkResponseToComment(recommendationResponse.ResponseId, comment.CommentId);
        }
    }
}