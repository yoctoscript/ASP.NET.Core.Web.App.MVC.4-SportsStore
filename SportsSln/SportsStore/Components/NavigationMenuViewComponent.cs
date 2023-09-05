using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    private IStoreRepository _repository;
    public NavigationMenuViewComponent(IStoreRepository repository)
    {
        _repository = repository;
    }
    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(_repository.Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .AsEnumerable<string>());
    }
}