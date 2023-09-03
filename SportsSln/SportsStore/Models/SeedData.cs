using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        StoreDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        if(!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Kayak", Description = "A boat for one person",
                    Category = "Watersports", Price = 275m
                },
                new Product
                {
                    Name = "Lifejacket", Description = "Protective and fashionable",
                    Category = "Watersports", Price = 48.95m
                },
                new Product
                {
                    Name = "Corner Flags", Description = "Gives your playing field a profesional touch",
                    Category = "Soccer", Price = 34.95m
                },
                new Product 
                {
                    Name = "Stadium", Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer", Price = 79500m
                },
                new Product
                {
                    Name = "Thinking Cap", Description = "Improve brain efficiency by 75%",
                    Category = "Chess", Price = 16m
                },
                new Product
                {
                    Name = "Unsteady Chair", Description = "Secretly gives your opponent a disadvantage",
                    Category = "Chess", Price = 29.95m
                },
                new Product
                {
                    Name = "Human Chess Board", Description = "A fun game for the family",
                    Category = "Chess", Price = 75
                },
                new Product
                {
                    Name = "Bling-Bling King", Description = "Gold-plated, diamond-studded King",
                    Category = "Chess", Price = 1200
                }
            );
            context.SaveChanges();
        }
    }
}