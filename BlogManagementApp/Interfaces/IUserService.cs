using BlogManagementApp.Models;
using BlogManagementApp.ViewModels;

namespace BlogManagementApp.Interfaces
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(RegisterViewModel model);
        Task<LoginResultViewModel> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
