using Cupid.With.Idiomatic;
using Cupid.With.Models;

namespace Cupid.With.Tests.Idiomatic;

/// <summary>
/// Tests du menu de café.
/// CUPID Idiomatic : l'API est naturelle en C#, pas de builder verbeux.
/// </summary>
public class CoffeeMenuTests
{
    [Fact]
    public void WhenEspressoSmallThenPriceIs250()
    {
        // Act
        var espresso = CoffeeMenu.Espresso(CoffeeSize.Small);

        // Assert
        Assert.Equal("Espresso", espresso.Name);
        Assert.Equal(CoffeeSize.Small, espresso.Size);
        Assert.Equal(2.50m, espresso.Price.Amount);
    }

    [Fact]
    public void WhenLatteMediumThenDefaultSizeIsMedium()
    {
        // Act
        // CUPID Idiomatic : paramètre par défaut, pas de surcharge
        var latte = CoffeeMenu.Latte();

        // Assert
        Assert.Equal(CoffeeSize.Medium, latte.Size);
        Assert.Equal(4.00m, latte.Price.Amount);
    }

    [Fact]
    public void WhenCappuccinoLargeThenPriceIs400()
    {
        // Act
        var cappuccino = CoffeeMenu.Cappuccino(CoffeeSize.Large);

        // Assert
        Assert.Equal(4.00m, cappuccino.Price.Amount);
    }

    // CUPID Idiomatic : créer une commande en une ligne, naturel et lisible
    [Fact]
    public void WhenCreateOrderLineThenQuantityAndTotalCorrect()
    {
        // Act
        var line = CoffeeMenu.OrderLine(CoffeeMenu.Latte(), 2);

        // Assert
        Assert.Equal(2, line.Quantity);
        Assert.Equal(8.00m, line.LineTotal.Amount);
    }

    // CUPID Idiomatic : comparer la lisibilité avec le builder Without
    //   Without : new OrderDtoBuilder().WithCustomerName("Alice").WithCustomerEmail("...").AddBeverage("Latte", 4.00m, 2, "Medium").Build()
    //   With    : new CoffeeOrder("Alice", "alice@coffee.com", [CoffeeMenu.OrderLine(CoffeeMenu.Latte(), 2)])
    [Fact]
    public void WhenCreateFullOrderThenReadableAndConcise()
    {
        // Act
        var order = new CoffeeOrder("Alice", "alice@coffee.com",
        [
            CoffeeMenu.OrderLine(CoffeeMenu.Espresso(), 1),
            CoffeeMenu.OrderLine(CoffeeMenu.Latte(CoffeeSize.Large), 2)
        ]);

        // Assert
        Assert.Equal(2, order.Lines.Count);
        Assert.Equal("Alice", order.CustomerName);
    }
}
