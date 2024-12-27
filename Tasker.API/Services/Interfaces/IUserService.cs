using Tasker.Domain.DTO;

namespace Tasker.API.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAllUsers();
    
    public Task<User> GetUserById(int id);
}