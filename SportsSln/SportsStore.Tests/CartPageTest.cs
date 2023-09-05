using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SportsStore.Tests;

public class CartPageTests
{
    [Fact]
    public void Can_Load_Cart()
    {
        // Arrange
        Product p1 = new() {ProductId = 1, Name = "P1"};
        Product p2 = new() {ProductId = 2, Name = "P2"};
        Mock<IStoreRepository> mockRepository = new();
        mockRepository.Setup(p => p.Products).Returns((new Product[] {p1 ,p2}).AsQueryable<Product>);
        Cart testCart = new();
        testCart.AddItem(p1, 2);
        testCart.AddItem(p2, 1);

        Mock<ISession> mockSession = new();
        byte[]? data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Cart>(testCart));
        mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));
        Mock<HttpContext> mockContext = new();
        mockContext.Setup(c =>c.Session).Returns(mockSession.Object);

        // Act
        CartModel cartModel = new(mockRepository.Object)
        {
            PageContext = new( new ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new(),
                ActionDescriptor = new()
            })
        };
        cartModel.OnGet("myUrl");

        // Assert
        Assert.Equal(2, cartModel.Cart?.Lines.Count());
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }

    [Fact]
    public void Can_Update_Cart()
    {
        // Arrange
        Mock<IStoreRepository> mockRepository = new();
        mockRepository.Setup(p => p.Products).Returns((new Product[] 
        {
            new Product {ProductId = 1, Name = "P1"}
        }).AsQueryable<Product>);
        Cart? testCart = new();
        Mock<ISession> mockSession = new();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>())).Callback<string, byte[]>((key, val) => 
        {
            testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
        });
        Mock<HttpContext> mockContext = new();
        mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

        // Act
        CartModel cartModel = new(mockRepository.Object)
        {
            PageContext = new PageContext (new ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new PageActionDescriptor()
            })
        };
        cartModel.OnPost(1, "myUrl");

        // Assert
        Assert.Single(testCart.Lines);
        Assert.Equal("P1", testCart.Lines.First().Product.Name);
        Assert.Equal(1, testCart.Lines.First().Quantity);

    }
}
