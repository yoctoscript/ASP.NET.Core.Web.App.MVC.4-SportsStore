namespace SportsStore.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }

    void SaveProduct(Product p);
    void CreateProduct(Product p);
    void DeleteProduct(Product p);
}