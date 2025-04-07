using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BlogManagementApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        [ValidateNever]
        public User User { get; set; }

        public int BlogPostId { get; set; }
        [ValidateNever]
        public BlogPost BlogPost { get; set; }
    }
}
