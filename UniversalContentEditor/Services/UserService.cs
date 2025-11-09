using Microsoft.AspNetCore.Identity;
using UniversalContentEditor.Data;
using UniversalContentEditor.Models;
using UniversalContentEditor.Repositories;

namespace UniversalContentEditor.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            return await _userRepository.UpdateUserAsync(userDto);
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var hasPassword = await _userManager.HasPasswordAsync(user);
            IdentityResult result;

            if (hasPassword)
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return false;
            }

            result = await _userManager.AddPasswordAsync(user, newPassword);
            return result.Succeeded;
        }
    }
}

