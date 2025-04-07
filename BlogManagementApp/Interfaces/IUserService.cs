using BlogManagementApp.Models;

namespace BlogManagementApp.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<User> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}
