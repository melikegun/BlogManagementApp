using Microsoft.AspNetCore.Identity;

namespace BlogManagementApp.Models
{
    public class User : IdentityUser
    {
        public ICollection<BlogPost> BlogPosts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
