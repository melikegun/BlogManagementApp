using BlogManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementApp.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // Rolleri oluştur
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin kullanıcıyı oluştur
            var adminEmail = "admin@site.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Kategoriler
            if (!context.Categories.Any())
            {
                var categoryNames = new[]
                {
                    "Yazılım", "C#", "ASP.NET", "MVC", "SQL Server", "Entity Framework",
                    "Frontend", "Backend", "Fullstack", "OOP", "Design Patterns", "SOLID",
                    "LINQ", "RESTful API", "Authentication", "Authorization", "Uygulama Tasarımı",
                    "Veritabanı", "Unit Test", "Temiz Kod"
                };

                foreach (var name in categoryNames)
                {
                    context.Categories.Add(new Category { Name = name });
                }
                await context.SaveChangesAsync();
            }

            // Bloglar
            if (!context.BlogPosts.Any())
            {
                var categories = await context.Categories.Take(10).ToListAsync();
                var blogs = new List<BlogPost>();

                for (int i = 1; i <= 10; i++)
                {
                    var content = $@"
## {i}. Blog: ASP.NET Core ile Modern Blog Uygulaması

Bu projede, **kapsamlı bir blog yönetim sistemi** geliştirilmiştir. Projenin temel hedefi; kullanıcıların blog yazabildiği, yorum yapabildiği, admin paneli ile sistemi yönetebildiği, güvenli ve şık bir web uygulaması oluşturmaktır.

---

###  Öne Çıkan Özellikler

- Kullanıcı Kayıt & Giriş (ASP.NET Identity)
- Blog Oluşturma, Düzenleme ve Silme
- Yorum Yapma & Yönetme
- Kategori Sistemi ve Filtreleme
- Markdown Destekli İçerik Formatı
- SweetAlert2 ile Modern Uyarılar
- Görsel Yükleme (Upload)
- Admin Panel: Dashboard, İstatistikler
- Tema Desteği (Light/Dark)
- Responsive Tasarım (Bootstrap 5)

---

###  Kullanılan Teknolojiler

| Katman       | Teknoloji                        |
|--------------|----------------------------------|
| Backend      | ASP.NET Core MVC                 |
| Database     | SQL Server, Entity Framework     |
| UI           | Razor Pages, Bootstrap 5         |
| Auth         | ASP.NET Identity, Roles          |
| Alert        | SweetAlert2                      |
| İçerik       | Markdig (Markdown Parser)        |
| Upload       | IFormFile + wwwroot/uploads/     |

---

###  Mimaride Yer Alan Yapılar

- `Models` → BlogPost, User, Category, Comment
- `Controllers` → BlogController, AdminController, AccountController
- `Services` → IBlogService, IUserService, ICommentService
- `Views` → Razor + Layout + Partial + Section yapısı
- `DbInitializer` → Veri Tohumlama işlemleri

---

Proje, **SOLID prensiplerine** uygun olarak geliştirilmiş olup, servis tabanlı bir yapı benimsenmiştir. Kod yapısı kolay geliştirilebilir, test edilebilir ve genişletilebilir şekilde organize edilmiştir.

---

**İyi okumalar! Projeyi keşfetmeye devam edin.**
";

                    blogs.Add(new BlogPost
                    {
                        Title = $"Örnek Blog Yazısı {i}",
                        Content = content,
                        CategoryId = categories[i % categories.Count].Id,
                        UserId = adminUser.Id,
                        PublishedDate = DateTime.Now.AddDays(-i),
                        ImageUrl = "/uploads/sample.jpg" // veya: $"https://picsum.photos/600/300?random={i}"
                    });
                }

                context.BlogPosts.AddRange(blogs);
                await context.SaveChangesAsync();
            }
        }
    }
}
