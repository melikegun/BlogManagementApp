using BlogManagementApp.Models;

public class BlogHomeViewModel
{
    public List<Category> Categories { get; set; }
    public List<BlogPost> BlogPosts { get; set; }
    public int? SelectedCategoryId { get; set; }
}
