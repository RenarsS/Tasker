using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services.Interfaces;

public interface IResponseService
{
    Task<Response> GetResponseById(int id);
    
    Task<Response> CreateResponse(Response response);

    Task CreateResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating);

    Task LinkResponseToComment(int responseId, int commentId);
    
    Task<IEnumerable<ResponseRetrievalRating>> GetUnratedResponseRetrievalRatings();

    Task UpdateResponseRetrievalRating(int ratingId, float rating);
}