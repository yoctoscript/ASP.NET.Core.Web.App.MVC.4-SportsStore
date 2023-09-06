using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository orderRepository;
    private Cart cart;
    public OrderController(IOrderRepository orderRepository, Cart cartService)
    {
        this.orderRepository = orderRepository;
        cart = cartService; 
    }
    public IActionResult Checkout()
    {
        return View(new Order());
    }
    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (cart.Lines.Count() == 0)
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        if (ModelState.IsValid)
        {
            order.Lines = cart.Lines.ToArray();
            orderRepository.SaveOrder(order);
            cart.Clear();
            return RedirectToPage("/Completed", new {orderId = order.OrderId});
        }
        else
        {
            return View();
        }
    }
}