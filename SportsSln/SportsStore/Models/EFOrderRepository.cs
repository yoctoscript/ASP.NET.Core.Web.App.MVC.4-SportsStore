using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public class EFOrderRepository : IOrderRepository
{
    private StoreDbContext context;
    public EFOrderRepository(StoreDbContext context)
    {
        this.context = context;
    }
    public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);

    public void SaveOrder(Order order)
    {
        context.AttachRange(order.Lines.Select(l => l.Product));
        if (order.OrderId == 0)
        {
            context.Orders.Add(order);
        }
        context.SaveChanges();
    }
}