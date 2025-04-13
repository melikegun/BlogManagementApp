using BlogManagementApp.Models;

namespace BlogManagementApp.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllAsync(); // Admin yorumları görüntülemek için
        Task AddCommentAsync(Comment comment); // Kullanıcı için
        Task DeleteAsync(int id, string? userId = null); // Admin veya kullanıcı
    }

}
