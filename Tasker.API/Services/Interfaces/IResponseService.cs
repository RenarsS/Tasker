using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services.Interfaces;

public interface IResponseService
{
    Task<Response> CreateResponse(Response response);

    Task CreateResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating);

    Task LinkResponseToComment(int responseId, int commentId);
}