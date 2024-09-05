namespace Order.API.ViewModels;

public class CreateOrderVM 
{
    public string BuyerId { get; set;}

    public IList<CreateOrderItemsVM> OrderItems { get; set; }
}

public class CreateOrderItemsVM
{
    public string ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; } 
}