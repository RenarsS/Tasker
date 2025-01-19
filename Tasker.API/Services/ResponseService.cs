using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services;

public class ResponseService(IResponseRepository responseRepository) : IResponseService
{
    public async Task<Response> GetResponseById(int id)
        => await responseRepository.GetResponseById(id);

    public async Task<Response> CreateResponse(Response response)
        => await responseRepository.InsertResponse(response);
    public async Task CreateResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating)
        => await responseRepository.InsertResponseRetrievalRating(responseRetrievalRating);
    
    public async Task LinkResponseToComment(int responseId, int commentId)
        => await responseRepository.LinkResponseToComment(responseId, commentId);

    public async Task<IEnumerable<ResponseRetrievalRating>> GetUnratedResponseRetrievalRatings()
        => await responseRepository.GetAllUnratedResponseRetrievalRatings();

    public async Task UpdateResponseRetrievalRating(int ratingId, float rating)
        => await responseRepository.UpdateResponseRetrievalRating(ratingId, rating);
}