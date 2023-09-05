using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;
public class HomeController : Controller
{
    private IStoreRepository _repository;
    public int PageSize = 4;
    public HomeController(IStoreRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index(string? category, int productPage = 1) 
        => View(new ProductListViewModel 
            {
                Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category is null ? _repository.Products.Count() : _repository.Products.Where(p => p.Category == category).Count()
                },
                CurrentCategory = category
            });

}