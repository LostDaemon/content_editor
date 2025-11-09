using MongoDB.Driver;
using Editor.Application.Dto;
using Editor.Application.Interfaces;
using Editor.Domain.Entities;

namespace Editor.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserRepository(IMongoDatabase database, string collectionName = "users")
        {
            _users = database.GetCollection<ApplicationUser>(collectionName);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _users.Find(_ => true).ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id ?? string.Empty,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id ?? string.Empty,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.Id, userDto.Id);
            var update = Builders<ApplicationUser>.Update
                .Set(u => u.UserName, userDto.UserName)
                .Set(u => u.Email, userDto.Email)
                .Set(u => u.Role, userDto.Role);

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                update = update.Set(u => u.NormalizedEmail, userDto.Email.ToUpperInvariant());
            }

            if (!string.IsNullOrEmpty(userDto.UserName))
            {
                update = update.Set(u => u.NormalizedUserName, userDto.UserName.ToUpperInvariant());
            }

            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}

