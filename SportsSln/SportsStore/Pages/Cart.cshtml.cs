using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages;

public class CartModel : PageModel
{
    private IStoreRepository _repository;
    public CartModel(IStoreRepository repository, Cart cartService)
    {
        _repository = repository;
        Cart = cartService;
    }
    public Cart? Cart {get; set;}
    public string ReturnUrl {get; set;} = "/";
    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl ?? "/";
        //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
    }

    public IActionResult OnPost(long productId, string returnUrl)
    {
        Product? product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product is not null)
        {
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart?.AddItem(product, 1);
            //HttpContext.Session.SetJson("cart", Cart);
        }
        return RedirectToPage(new {returnUrl = returnUrl});
    }

    public IActionResult OnPostRemove(long productId, string returnUrl)
    {
        Cart?.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
        return RedirectToPage(new {returnUrl = returnUrl});
    }
}