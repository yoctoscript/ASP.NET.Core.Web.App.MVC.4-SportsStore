using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
    [Fact]
    public void Can_Select_Categories()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(p => p.Products).Returns((new Product[]
        {
            new Product {Category="Z"},
            new Product {Category="V"},
            new Product {Category="F"},
            new Product {Category="Z"},
            new Product {Category="A"},
            new Product {Category="B"}
        }).AsQueryable<Product>);
        NavigationMenuViewComponent navMenu = new(mock.Object);

        // Act
        string[] results = ((navMenu.Invoke() as ViewViewComponentResult)?.ViewData?.Model as IEnumerable<string>)?.ToArray() ?? Array.Empty<string>();

        // Assert
        Assert.True(Enumerable.SequenceEqual<string>(new string[]{"A", "B", "F", "V", "Z"}, results));
    }

    [Fact]
    public void Indicates_Selected_Category()
    {
        // Arrange
        string categoryToSelect = "First";
        Mock<IStoreRepository> mock = new();
        mock.Setup(p => p.Products).Returns((new Product[]
        {
            new Product {Category = "First"},
            new Product {Category = "Second"}
        }).AsQueryable<Product>());
        NavigationMenuViewComponent navMenu = new(mock.Object);
        navMenu.ViewComponentContext = new()
        {
            ViewContext = new()
            {
                RouteData = new()
            }
        };
        navMenu.RouteData.Values["category"] = categoryToSelect;

        // Act
        string result = (navMenu.Invoke() as ViewViewComponentResult)?.ViewData?["SelectedCategory"] as string ?? String.Empty;

        // Assert
        Assert.Equal(categoryToSelect, result);

    }
}