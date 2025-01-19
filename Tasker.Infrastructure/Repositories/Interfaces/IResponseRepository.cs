using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IResponseRepository
{
    Task<Response> GetResponseById(int id);
    
    Task<Response> InsertResponse(Response response);
    
    Task InsertResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating);
    
    Task LinkResponseToComment(int responseId, int commentId);
    
    Task UpdateResponseRetrievalRating(int ratingId, float rating);
    
    Task<IEnumerable<ResponseRetrievalRating>> GetAllUnratedResponseRetrievalRatings();

}