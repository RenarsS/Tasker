namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IRepository
{
    Task LinkToVector(int id, string vectorId);
}