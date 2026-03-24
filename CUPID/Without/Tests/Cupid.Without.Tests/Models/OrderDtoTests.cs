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
        // Arrange
        var order = new OrderDto();

        // Act
        // ANTI-CUPID Idiomatic : verbeux comparé à un record constructor
        order.SetCustomerEmail("alice@coffee.com");

        // Assert
        Assert.Equal("alice@coffee.com", order.GetCustomerEmail());
    }

    [Fact]
    public void WhenAddItemThenItemIsInList()
    {
        // Arrange
        var order = new OrderDto();
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");

        // Act
        order.AddItem(item);

        // Assert
        Assert.Single(order.GetItems());
        Assert.Equal("Espresso", order.GetItems()[0].GetName());
    }

    // ANTI-CUPID Predictable : le modèle est mutable — on peut changer l'état à tout moment
    [Fact]
    public void WhenItemMutatedAfterAddThenOrderReflectsChange()
    {
        // Arrange
        var order = new OrderDto();
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");
        order.AddItem(item);

        // Act
        item.SetName("Latte");

        // Assert
        // Surprise ! L'article dans la commande a changé de nom
        Assert.Equal("Latte", order.GetItems()[0].GetName());
    }
}
