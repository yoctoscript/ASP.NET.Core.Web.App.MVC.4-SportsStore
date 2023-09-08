using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(
    builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
var app = builder.Build();

if(app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
}
app.UseRequestLocalization(options =>
{
    options.AddSupportedCultures("en-US")
    .AddSupportedUICultures("en-US")
    .SetDefaultCulture("en-US");
});
app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("categoryPage", "{category}/Page{productPage}", new {Controller="Home", action="Index"});
app.MapControllerRoute("page", "Page{productPage}", new {Controller="Home", action="Index"});
app.MapControllerRoute("category", "{category}", new {Controller="Home", action="Index"});
app.MapControllerRoute("pagination", "Products/Page{productPage}", new {Controller ="Home", action="Index"});

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");


SeedData.EnsurePopulated(app);
IdentitySeedData.EnsurePopulated(app);
app.Run();
