using UniversalContentEditor.Data;
using UniversalContentEditor.Models;

namespace UniversalContentEditor.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(UserDto userDto);
    }
}

