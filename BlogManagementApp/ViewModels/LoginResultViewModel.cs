using BlogManagementApp.Models;

namespace BlogManagementApp.ViewModels
{
    public class LoginResultViewModel
    {
        public User? User { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAdmin { get; set; }
    }
}
