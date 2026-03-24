using Cupid.Without.Models;

namespace Cupid.Without.Tests.Models;

/// <summary>
/// Tests des modèles anémiques Java-style.
/// Remarquer la verbosité : chaque propriété nécessite Get/Set au lieu d'un constructeur.
/// </summary>
public class OrderDtoTests
{
    [Fact]
    public void WhenSetCustomerEmailThenGetReturnsIt()
    {
        var order = new OrderDto();

        // ANTI-CUPID Idiomatic : verbeux comparé à un record constructor
        order.SetCustomerEmail("alice@coffee.com");

        Assert.Equal("alice@coffee.com", order.GetCustomerEmail());
    }

    [Fact]
    public void WhenAddItemThenItemIsInList()
    {
        var order = new OrderDto();
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");

        order.AddItem(item);

        Assert.Single(order.GetItems());
        Assert.Equal("Espresso", order.GetItems()[0].GetName());
    }

    // ANTI-CUPID Predictable : le modèle est mutable — on peut changer l'état à tout moment
    [Fact]
    public void WhenItemMutatedAfterAddThenOrderReflectsChange()
    {
        var order = new OrderDto();
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");
        order.AddItem(item);

        item.SetName("Latte");

        // Surprise ! L'article dans la commande a changé de nom
        Assert.Equal("Latte", order.GetItems()[0].GetName());
    }
}
