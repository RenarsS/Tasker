using Tasker.Domain.DTO;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    
    Task<User> GetUserById(int id);
}