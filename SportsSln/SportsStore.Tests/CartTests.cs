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
public class CartTests
{
    [Fact]
    public void Can_Add_New_Lines()
    {
        // Arrange
        Product p1 = new() {ProductId = 0, Name="Sheet"};
        Product p2 = new() {ProductId = 1, Name="Style"};
        Cart cart = new();

        // Act
        cart.AddItem(p1, 5);
        cart.AddItem(p2, 3);
        CartLine[] result = cart.Lines.ToArray();

        // Assert
        Assert.Equal(5, result[0].Quantity);
        Assert.Equal(3, result[1].Quantity);
        Assert.True(result[0].Product.Name == "Sheet");
        Assert.True(result[1].Product.Name == "Style");
        Assert.Equal(0, result[0].Product.ProductId);
        Assert.Equal(1, result[1].Product.ProductId);
    }

    [Fact]
    public void Can_Add_Quantity_For_Existing_Lines()
    {
        // Arrange
        Product p1 = new() {ProductId = 0, Name="Sheet"};
        Product p2 = new() {ProductId = 1, Name="Style"};
        Cart cart = new();

        // Act
        cart.AddItem(p1, 5);
        cart.AddItem(p1, 5);
        cart.AddItem(p1, 5);
        cart.AddItem(p2, 3);
        cart.AddItem(p2, 11);

        CartLine[] result = cart.Lines.ToArray();

        // Assert
        Assert.Equal(15, result[0].Quantity);
        Assert.Equal(14, result[1].Quantity);
    }

    [Fact]
    public void Can_Remove_Line()
    {
        // Arrange
        Product product = new() {ProductId = 0, Name="Will be deleted"};
        Product product1 = new() {ProductId = 1, Name="Will not be deleted"};

        Cart cart = new();
        cart.AddItem(product, 6);
        cart.AddItem(product1, 2);

        // Act
        cart.RemoveLine(product);

        // Assert
        Assert.Empty(cart.Lines.Where(cl => cl.Product == product));
        Assert.Empty(cart.Lines.Where(cl => cl.Product.ProductId == 0));
        Assert.Single(cart.Lines);
    }

    [Fact]
    public void Calculate_Cart_Total()
    {
        // Arrange
        Product p1 = new() {ProductId = 0, Price = 100m};
        Product p2 = new() {ProductId = 1, Price = 200m};
        Product p3 = new() {ProductId = 2, Price = 300m};
        Cart cart = new();
        cart.AddItem(p1, 1);
        cart.AddItem(p2, 1);
        cart.AddItem(p3, 1);
        cart.AddItem(p2, 2);

        // Act

        // Assert
        Assert.Equal(1000m, cart.ComputeTotalValue());
    }

    [Fact]
    public void Can_Clear_Items()
    {
        // Arrange
        Product p1 = new() {ProductId = 0, Price = 100m};
        Product p2 = new() {ProductId = 1, Price = 200m};
        Product p3 = new() {ProductId = 2, Price = 300m};
        Cart cart = new();
        cart.AddItem(p1, 1);
        cart.AddItem(p2, 1);
        cart.AddItem(p3, 1);
        cart.AddItem(p2, 2);

        // Act
        cart.Clear();

        // Assert
        Assert.Empty(cart.Lines);
    }
}