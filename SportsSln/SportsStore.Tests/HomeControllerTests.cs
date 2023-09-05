using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;
using Xunit;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange.
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new Product { Name = "Static Shiv", Description = "OP Item for farming", Category = "Attack Speed", Price = 2700m},
            new Product { Name = "Blade of the ruined king", Description = "Nice for vayne", Category = "lifesteal/attack speed", Price=3200m}
        }).AsQueryable<Product>);
        HomeController homeController = new HomeController(mock.Object);

        // Act.
        ProductListViewModel? model = (homeController.Index(category: null, productPage:1) as ViewResult)?.ViewData.Model as ProductListViewModel;
    
        // Assert.
        Product[] modelArray = model?.Products.ToArray() ?? Array.Empty<Product>();
        Assert.Equal("Static Shiv", modelArray[0].Name);
        Assert.Equal(3200m, modelArray[1].Price);
    }

    [Fact]
    public void Can_Paginate()
    {
        // Arrange.
        Mock<IStoreRepository> mock = new();
        mock.Setup(p => p.Products).Returns((new Product[]
        {
            new Product {ProductId = 1, Name = "One"},
            new Product {ProductId = 2, Name = "Two"},
            new Product {ProductId = 3, Name = "Three"},
            new Product {ProductId = 4, Name = "Four"},
            new Product {ProductId = 5, Name = "Five"},
            new Product {ProductId = 6, Name = "Six"}
        }).AsQueryable<Product>);
        HomeController homeController = new(mock.Object);
        homeController.PageSize = 3;

        // Act.
        ProductListViewModel? Model = (homeController.Index(category: null, productPage:2) as ViewResult)?.ViewData.Model as ProductListViewModel;
        Product[] modelArray = Model?.Products.ToArray() ?? Array.Empty<Product>();
        
        // Assert
        Assert.Equal("Four", modelArray[0].Name);
        Assert.Equal(5, modelArray[1].ProductId);
        Assert.Equal(6, modelArray[2].ProductId);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        // Arrange. 
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(
            (new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }).AsQueryable<Product>
        );
        HomeController homeController = new(mock.Object) {PageSize = 3};

        // Act.
        ProductListViewModel? result = (homeController.Index(category: null, productPage:2) as ViewResult)?.ViewData.Model as ProductListViewModel;
    
        // Assert.
        PagingInfo? pageInfo = result?.PagingInfo;
        Assert.Equal(3, pageInfo?.ItemsPerPage);
        Assert.Equal(2, pageInfo?.CurrentPage);
        Assert.Equal(2, pageInfo?.TotalPages);
        Assert.Equal(5, pageInfo?.TotalItems);  
    }

    [Fact]
    public void Can_Filter_Products()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(p => p.Products).Returns((new Product[]
        {
            new Product {Name = "P1", Category = "Odd"},
            new Product {Name = "P2", Category = "Even"},
            new Product {Name = "P3", Category = "Odd"},
            new Product {Name = "P4", Category = "Even"},
            new Product {Name = "P5", Category = "Odd"},
            new Product {Name = "P6", Category = "Even"}
        }).AsQueryable<Product>);
        HomeController homeController = new HomeController(mock.Object);
        homeController.PageSize = 3;

        // Act
        Product[] products = ((homeController.Index(category: "Odd", productPage: 1) as ViewResult)?.ViewData.Model as ProductListViewModel)?.Products.ToArray()!;

        // Assert
        Assert.Equal(3, products.Length);
        Assert.True("Odd" == products[0].Category);
        Assert.True("Odd" == products[1].Category);
        Assert.True("Odd" == products[2].Category);
    }

    [Fact]
    public void Generate_Specific_Category_Product_Count()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(p => p.Products).Returns((new Product[]
        {
            new Product {Category = "A"},
            new Product {Category = "B"},
            new Product {Category = "A"},
            new Product {Category = "D"},
            new Product {Category = "A"},
            new Product {Category = "A"},
            new Product {Category = "A"},
            new Product {Category = "A"},

        }).AsQueryable<Product>);
        HomeController homeController = new(mock.Object);

        // Act
        ProductListViewModel result = (homeController.Index(category:null, productPage: 1) as ViewResult)?.ViewData.Model as ProductListViewModel ?? new ProductListViewModel();
    
        // Assert
        Assert.Equal(8, result.PagingInfo.TotalItems);
    }
}