using Microsoft.AspNetCore.Identity;

namespace BlogManagementApp.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; }  // Admin, User vb.
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
