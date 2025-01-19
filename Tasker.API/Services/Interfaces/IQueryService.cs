using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services.Interfaces;

public interface IQueryService
{
    Task<Query> GetQueryById(int id);
    
    Task<Query> CreateQuery(string prompt, Query query);

    Task CreateQueryResponseRating(QueryResponseRating queryResponseRating);

    Task<IEnumerable<QueryResponseRating>> GetUnratedQueryResponseRatings();
    Task UpdateQueryResponseRating(int ratingId, float rating);
}