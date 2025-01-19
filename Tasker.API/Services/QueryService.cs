using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services;

public class QueryService(
    IQueryRepository queryRepository,
    IEmbeddingProcessor embeddingProcessor) : IQueryService
{
    public async Task<Query> GetQueryById(int id)
        => await queryRepository.GetQueryById(id);

    public async Task<Query> CreateQuery(string prompt, Query query)
    {
        var insertedQuery = await queryRepository.InsertQuery(query);
        var queryVectorId = await embeddingProcessor.ProcessQuery(prompt, insertedQuery);
        await queryRepository.LinkToVector(insertedQuery.QueryId, queryVectorId);
        return insertedQuery;
    }
        

    public async Task CreateQueryResponseRating(QueryResponseRating queryResponseRating)
        => await queryRepository.InsertQueryResponseRating(queryResponseRating);

    public async Task<IEnumerable<QueryResponseRating>> GetUnratedQueryResponseRatings()
        => await queryRepository.GetAllUnratedQueryResponseRatings();

    public async  Task UpdateQueryResponseRating(int ratingId, float rating)
        => await queryRepository.UpdateQueryResponseRating(ratingId, rating);
}