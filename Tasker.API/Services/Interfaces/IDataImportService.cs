using Tasker.Domain.Import;

namespace Tasker.API.Services.Interfaces;

public interface IDataImportService
{
    Task<(int, int, int)> ImportDataBatches(IEnumerable<DataBatch> batches);
    
    Task<(int, int, int)> ImportDataBatch(DataBatch batch);
}