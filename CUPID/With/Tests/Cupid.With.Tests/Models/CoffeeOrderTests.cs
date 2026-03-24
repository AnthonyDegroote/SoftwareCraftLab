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
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));
        var line = new OrderLine(espresso, 3);

        // CUPID Predictable : propriété calculée, toujours le même résultat
        Assert.Equal(7.50m, line.LineTotal.Amount);
    }

    [Fact]
    public void WhenCoffeeOrderCreatedThenImmutable()
    {
        var latte = new Coffee("Latte", CoffeeSize.Medium, new Money(4.00m));
        var order = new CoffeeOrder("Alice", "alice@coffee.com", [new OrderLine(latte, 2)]);

        // CUPID Idiomatic : records immuables, pas de mutation possible
        Assert.Equal("Alice", order.CustomerName);
        Assert.Equal("alice@coffee.com", order.CustomerEmail);
        Assert.Single(order.Lines);
    }

    // CUPID Idiomatic : avec `with`, on crée une copie modifiée sans muter l'original
    [Fact]
    public void WhenUsingWithExpressionThenOriginalUnchanged()
    {
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));

        var large = espresso with { Size = CoffeeSize.Large, Price = new Money(3.50m) };

        Assert.Equal(2.50m, espresso.Price.Amount);
        Assert.Equal(3.50m, large.Price.Amount);
    }
}
