namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IRepository
{
    System.Threading.Tasks.Task LinkToVector(int id, string vectorId);
}