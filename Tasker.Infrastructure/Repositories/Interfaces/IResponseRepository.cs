using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IResponseRepository
{
    Task<Response> InsertResponse(Response response);
    
    Task InsertQueryResponseRating(ResponseRetrievalRating responseRetrievalRating);
    
    Task LinkResponseToComment(int responseId, int commentId);
}