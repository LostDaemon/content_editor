using UniversalContentEditor.Models;

namespace UniversalContentEditor.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> ChangePasswordAsync(string userId, string newPassword);
    }
}

