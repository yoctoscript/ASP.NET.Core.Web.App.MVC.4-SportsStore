namespace SportsStore.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }
}