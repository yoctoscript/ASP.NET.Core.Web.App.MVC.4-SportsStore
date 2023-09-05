using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();
app.UseStaticFiles();
app.UseSession();
app.MapControllerRoute("categoryPage", "{category}/Page{productPage}", new {Controller="Home", action="Index"});
app.MapControllerRoute("page", "Page{productPage}", new {Controller="Home", action="Index"});
app.MapControllerRoute("category", "{category}", new {Controller="Home", action="Index"});
app.MapControllerRoute("pagination", "Products/Page{productPage}", new {Controller ="Home", action="Index"});

app.MapDefaultControllerRoute();
app.MapRazorPages();
SeedData.EnsurePopulated(app);
app.Run();
