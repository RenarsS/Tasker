using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IQueryRepository : IRepository
{
    Task<Query> InsertQuery(Query query);
    
    Task InsertQueryResponseRating(QueryResponseRating queryResponseRating);
    
}