using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services;

public class ResponseService(IResponseRepository responseRepository) : IResponseService
{
    public async Task<Response> CreateResponse(Response response)
        => await responseRepository.InsertResponse(response);
    public async Task CreateResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating)
        => await responseRepository.InsertQueryResponseRating(responseRetrievalRating);
    
    public async Task LinkResponseToComment(int responseId, int commentId)
        => await responseRepository.LinkResponseToComment(responseId, commentId);
}