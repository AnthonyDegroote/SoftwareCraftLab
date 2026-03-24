using Cupid.With.Models;

namespace Cupid.With.Tests.Models;

/// <summary>
/// Tests des modèles de commande.
/// CUPID Domain : les tests parlent le langage du coffee shop.
/// CUPID Predictable : les records immuables rendent les tests simples.
/// </summary>
public class CoffeeOrderTests
{
    [Fact]
    public void WhenOrderLineCreatedThenLineTotalIsCalculated()
    {
        // Arrange
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));

        // Act
        var line = new OrderLine(espresso, 3);

        // Assert
        // CUPID Predictable : propriété calculée, toujours le même résultat
        Assert.Equal(7.50m, line.LineTotal.Amount);
    }

    [Fact]
    public void WhenCoffeeOrderCreatedThenImmutable()
    {
        // Arrange
        var latte = new Coffee("Latte", CoffeeSize.Medium, new Money(4.00m));

        // Act
        var order = new CoffeeOrder("Alice", "alice@coffee.com", [new OrderLine(latte, 2)]);

        // Assert
        // CUPID Idiomatic : records immuables, pas de mutation possible
        Assert.Equal("Alice", order.CustomerName);
        Assert.Equal("alice@coffee.com", order.CustomerEmail);
        Assert.Single(order.Lines);
    }

    // CUPID Idiomatic : avec `with`, on crée une copie modifiée sans muter l'original
    [Fact]
    public void WhenUsingWithExpressionThenOriginalUnchanged()
    {
        // Arrange
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));

        // Act
        var large = espresso with { Size = CoffeeSize.Large, Price = new Money(3.50m) };

        // Assert
        Assert.Equal(2.50m, espresso.Price.Amount);
        Assert.Equal(3.50m, large.Price.Amount);
    }
}
