using Cupid.With.Models;
using Cupid.With.Unix;

namespace Cupid.With.Tests.Unix;

/// <summary>
/// Tests du validateur focalisé.
/// CUPID Unix : pas de configuration, pas de framework — juste valider une commande.
/// </summary>
public class OrderValidatorTests
{
    private readonly OrderValidator _validator = new();

    private static CoffeeOrder CreateValidOrder() =>
        new("Alice", "alice@coffee.com",
            [new OrderLine(new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m)), 1)]);

    [Fact]
    public void WhenValidOrderThenSuccess()
    {
        // Act
        var result = OrderValidator.Validate(CreateValidOrder());

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void WhenMissingCustomerNameThenFailure()
    {
        // Arrange
        var order = CreateValidOrder() with { CustomerName = "" };

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("nom", result.Error);
    }

    [Fact]
    public void WhenMissingEmailThenFailure()
    {
        // Arrange
        var order = CreateValidOrder() with { CustomerEmail = "" };

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("e-mail", result.Error);
    }

    [Fact]
    public void WhenNoCoffeesThenFailure()
    {
        // Arrange
        var order = CreateValidOrder() with { Lines = [] };

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("au moins un café", result.Error);
    }

    [Fact]
    public void WhenZeroQuantityThenFailure()
    {
        // Arrange
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));
        var order = new CoffeeOrder("Alice", "alice@coffee.com",
            [new OrderLine(espresso, 0)]);

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("quantité", result.Error);
    }

    [Fact]
    public void WhenNullOrderThenThrows() => Assert.Throws<ArgumentNullException>(() => OrderValidator.Validate(null!));
}
