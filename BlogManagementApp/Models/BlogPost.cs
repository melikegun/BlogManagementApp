using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
