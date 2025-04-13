using BlogManagementApp.Models;
using BlogManagementApp.ViewModels;
using BlogManagementApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlogManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User?> RegisterAsync(RegisterViewModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return user;
            }

            return null;
        }

        public async Task<LoginResultViewModel> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    return new LoginResultViewModel
                    {
                        User = user,
                        IsSuccess = true,
                        IsAdmin = isAdmin
                    };
                }
            }

            return new LoginResultViewModel
            {
                User = null,
                IsSuccess = false,
                IsAdmin = false
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
