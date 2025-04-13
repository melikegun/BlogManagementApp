# Blog Management App

Blog Management App, kullanıcıların blog yazıları oluşturup yönetebildiği, kategori filtreleme ve yorum yapma işlemlerini gerçekleştirebildiği bir ASP.NET Core MVC tabanlı web uygulamasıdır. Admin paneli üzerinden ise sistem genelinde yönetim sağlanmaktadır.

## Özellikler

- Kullanıcı kayıt ve giriş işlemleri (ASP.NET Identity ile)
- Blog oluşturma, düzenleme ve silme
- Bloglara kategori atama ve filtreleme
- Yorum ekleme ve silme
- Markdown destekli içerik yazımı
- SweetAlert2 ile modern uyarılar
- Tema değiştirme (light/dark mod)
- Admin paneli ile:
  - Kullanıcı, blog ve yorum sayıları
  - Son yorumların listelenmesi
  - Blog ve yorum yönetimi
  - Kategori yönetimi
- Bootstrap ile responsive tasarım

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- ASP.NET Identity (Role tabanlı yetkilendirme)
- Razor View Engine
- Bootstrap 5
- SweetAlert2
- Markdig (Markdown desteği)
- IFormFile ile dosya yükleme işlemleri

## Proje Mimarisi

- **Models**: Entity sınıfları (User, BlogPost, Comment, Category)
- **Controllers**: İş mantığını yöneten controllerlar (Blog, Admin, Account)
- **Views**: Razor tabanlı kullanıcı arayüzleri
- **Services**: Katmanlı yapı için oluşturulmuş servis arayüzleri ve implementasyonları
- **Data**: Uygulama veritabanı context sınıfı (`ApplicationDbContext`) ve ilk veri yükleyici (`DbInitializer`)

## Başlangıç

1. Projeyi klonlayın.
2. `appsettings.json` içerisinde veritabanı bağlantınızı yapılandırın.
3. NuGet paketlerini yükleyin (`Restore`).
4. `Update-Database` komutuyla veritabanını oluşturun.
5. Uygulamayı başlatın.

## Varsayılan Admin Girişi

- **E-posta**: admin@site.com  
- **Şifre**: Admin123!

## Notlar

- Projede veri tohumlama (`DbInitializer.cs`) ile örnek blog, kategori ve admin kullanıcısı oluşturulmuştur.
- Görseller `wwwroot/uploads` klasörüne yüklenmektedir. Varsayılan örnek bloglarda örnek görsel yolu `/uploads/sample.jpeg` olarak tanımlıdır.

## Lisans

Bu proje eğitim ve portföy amaçlı geliştirilmiştir.
