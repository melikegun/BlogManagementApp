using BlogManagementApp.Models;
using System.Collections.Generic;

namespace BlogManagementApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalBlogs { get; set; }
        public int TotalComments { get; set; }
        public List<Comment> RecentComments { get; set; }
    }
}
