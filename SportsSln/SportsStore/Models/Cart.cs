namespace SportsStore.Models;

public class Cart
{
    public List<CartLine> Lines {get; set;} = new();
    public virtual void AddItem(Product product, int quantity)
    {
        CartLine? line = Lines
            .Where(l => l.Product.ProductId == product.ProductId)
            .FirstOrDefault();
        if (line is null)
        {
            Lines.Add(new CartLine
            {
               Product = product,
               Quantity = quantity 
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public virtual void RemoveLine(Product product)
    {
        Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
    }

    public decimal ComputeTotalValue()
    {
        return Lines.Sum(l => l.Product.Price * l.Quantity);
    }

    public virtual void Clear()
    {
        Lines.Clear();
    }


    
}

public class CartLine
    {
        public int CartLineID { get; set;}
        public Product Product { get; set;} = new();
        public int Quantity { get; set;}
    }