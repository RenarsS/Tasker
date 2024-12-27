using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Repositories;

namespace Tasker.API.Services;

public class UserService(UserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await userRepository.GetUsers();
    }

    public async Task<User> GetUserById(int id)
    {
        return await userRepository.GetUserById(id);
    }
}