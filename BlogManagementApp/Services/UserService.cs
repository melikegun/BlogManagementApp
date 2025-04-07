using BlogManagementApp.Data;
using BlogManagementApp.Models;
using BlogManagementApp.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BlogManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public Task LogoutAsync()
        {
            // Bu kısım ileride cookie/session yönetimi eklenince yazılacak
            return Task.CompletedTask;
        }
    }
}
