using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
