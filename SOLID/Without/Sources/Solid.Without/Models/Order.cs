namespace Solid.Without.Models;

public record OrderItem(string ProductName, decimal UnitPrice, int Quantity);

public record Order(string CustomerEmail, List<OrderItem> Items, string? DiscountCode = null);
