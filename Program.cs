using Microsoft.EntityFrameworkCore; // <--- Thêm thư viện này
using WebBanHang.Models;           // <--- Thêm namespace Models của bạn

var builder = WebApplication.CreateBuilder(args);

// --- THÊM ĐOẠN NÀY ĐỂ KẾT NỐI DATABASE ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ----------------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();
// --- THÊM ĐOẠN NÀY ĐỂ BẬT SESSION ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian nhớ giỏ hàng (30 phút)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// ------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession(); // <--- THÊM DÒNG NÀY VÀO
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// --- TỰ ĐỘNG THÊM DỮ LIỆU MẪU KHI KHỞI ĐỘNG ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Đảm bảo database đã được tạo
    context.Database.EnsureCreated();

    // Nếu chưa có danh mục nào thì thêm mẫu
    if (!context.Categories.Any())
    {
        var category = new Category { Name = "Dụng cụ Golf", Description = "Gậy, bóng và phụ kiện golf" };
        context.Categories.Add(category);
        context.SaveChanges();

        context.Products.AddRange(
            new Product { Name = "Gậy Driver TaylorMade", Price = 12500000, StockQuantity = 10, Description = "Gậy golf chính hãng đời mới", CategoryId = category.Id, ImageUrl = "https://via.placeholder.com/300x200" },
            new Product { Name = "Túi đựng gậy Golf Titleist", Price = 3500000, StockQuantity = 15, Description = "Túi da cao cấp chống thấm nước", CategoryId = category.Id, ImageUrl = "https://via.placeholder.com/300x200" }
        );
        context.SaveChanges();
    }
}
// ---------------------------------------------
app.Run();