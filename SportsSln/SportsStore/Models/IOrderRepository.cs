using SportsStore.Models;

public interface IOrderRepository
{
    public IQueryable<Order> Orders {get;}
    public void SaveOrder(Order order);
}