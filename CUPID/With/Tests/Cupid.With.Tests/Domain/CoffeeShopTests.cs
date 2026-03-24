using Cupid.With.Composable;
using Cupid.With.Domain;
using Cupid.With.Idiomatic;
using Cupid.With.Models;
using Cupid.With.Unix;

namespace Cupid.With.Tests.Domain;

/// <summary>
/// Tests de la façade CoffeeShop.
/// CUPID Domain : les tests parlent le langage du coffee shop.
/// "PlaceOrder", "SaveOrder", "SendConfirmation" — pas "ProcessEntity", "DispatchPayload".
/// </summary>
public class CoffeeShopTests
{
    private static CoffeeShop CreateCoffeeShop(
        out InMemoryOrderStore store,
        out InMemoryConfirmationNotifier notifier)
    {
        store = new InMemoryOrderStore();
        notifier = new InMemoryConfirmationNotifier();
        return new CoffeeShop(
            new OrderValidator(),
            new OrderPricing(),
            new TaxCalculation(0.20m),
            store,
            notifier);
    }

    private static CoffeeOrder CreateOrder() =>
        new("Alice", "alice@coffee.com",
        [
            CoffeeMenu.OrderLine(CoffeeMenu.Espresso(), 2),
            CoffeeMenu.OrderLine(CoffeeMenu.Latte(), 1)
        ]);

    // CUPID Domain : "PlaceOrder" est le verbe métier
    [Fact]
    public void WhenPlaceOrderThenConfirmationContainsCorrectTotal()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);
        var order = CreateOrder();

        // Act
        var confirmation = shop.PlaceOrder(order);

        // Assert
        // Espresso 2.50×2 + Latte 4.00×1 = 9.00
        Assert.Equal(9.00m, confirmation.SubTotal.Amount);
    }

    [Fact]
    public void WhenPlaceOrderThenTaxIsCalculated()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);

        // Act
        var confirmation = shop.PlaceOrder(CreateOrder());

        // Assert
        // 9.00 × 0.20 = 1.80
        Assert.Equal(1.80m, confirmation.Tax.Amount);
    }

    [Fact]
    public void WhenPlaceOrderThenTotalIncludesTax()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);

        // Act
        var confirmation = shop.PlaceOrder(CreateOrder());

        // Assert
        // 9.00 + 1.80 = 10.80
        Assert.Equal(10.80m, confirmation.Total.Amount);
    }

    // CUPID Domain : "SaveOrder" — langage métier
    [Fact]
    public void WhenPlaceOrderThenOrderIsSaved()
    {
        // Arrange
        var shop = CreateCoffeeShop(out var store, out _);

        // Act
        shop.PlaceOrder(CreateOrder());

        // Assert
        Assert.Single(store.Orders);
    }

    // CUPID Domain : "SendConfirmation" — langage métier
    [Fact]
    public void WhenPlaceOrderThenConfirmationIsSent()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out var notifier);

        // Act
        shop.PlaceOrder(CreateOrder());

        // Assert
        Assert.Single(notifier.SentConfirmations);
        Assert.Equal("alice@coffee.com", notifier.SentConfirmations[0].Order.CustomerEmail);
    }

    [Fact]
    public void WhenInvalidOrderThenThrows()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);
        var invalidOrder = new CoffeeOrder("", "alice@coffee.com",
            [CoffeeMenu.OrderLine(CoffeeMenu.Espresso(), 1)]);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => shop.PlaceOrder(invalidOrder));

        // Assert
        Assert.Contains("nom", exception.Message);
    }

    [Fact]
    public void WhenNullOrderThenThrows()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => shop.PlaceOrder(null!));
    }

    // CUPID Composable : PlaceOrder compose pricing + tax + store + notifier
    // sans contexte mutable partagé, chaque résultat est passé en paramètre au suivant
    [Fact]
    public void WhenPlaceOrderThenConfirmationContainsOriginalOrder()
    {
        // Arrange
        var shop = CreateCoffeeShop(out _, out _);
        var order = CreateOrder();

        // Act
        var confirmation = shop.PlaceOrder(order);

        // Assert
        Assert.Equal(order, confirmation.Order);
    }
}
