using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IQueryRepository : IRepository
{
    Task<Query> GetQueryById(int id);
    
    Task<Query> InsertQuery(Query query);
    
    Task InsertQueryResponseRating(QueryResponseRating queryResponseRating);
    
    Task UpdateQueryResponseRating(int ratingId, float rating);

    Task<IEnumerable<QueryResponseRating>> GetAllUnratedQueryResponseRatings();

}