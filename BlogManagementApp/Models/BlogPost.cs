using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik alanı boş bırakılamaz.")]
        public string Content { get; set; }

        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "Kategori seçilmesi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir kategori seçin.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
