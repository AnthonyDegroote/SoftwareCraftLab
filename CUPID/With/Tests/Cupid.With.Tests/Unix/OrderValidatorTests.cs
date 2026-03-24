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
        var result = _validator.Validate(CreateValidOrder());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void WhenMissingCustomerNameThenFailure()
    {
        var order = CreateValidOrder() with { CustomerName = "" };

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        Assert.Contains("nom", result.Error);
    }

    [Fact]
    public void WhenMissingEmailThenFailure()
    {
        var order = CreateValidOrder() with { CustomerEmail = "" };

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        Assert.Contains("e-mail", result.Error);
    }

    [Fact]
    public void WhenNoCoffeesThenFailure()
    {
        var order = CreateValidOrder() with { Lines = [] };

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        Assert.Contains("au moins un café", result.Error);
    }

    [Fact]
    public void WhenZeroQuantityThenFailure()
    {
        var espresso = new Coffee("Espresso", CoffeeSize.Small, new Money(2.50m));
        var order = new CoffeeOrder("Alice", "alice@coffee.com",
            [new OrderLine(espresso, 0)]);

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        Assert.Contains("quantité", result.Error);
    }

    [Fact]
    public void WhenNullOrderThenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => _validator.Validate(null!));
    }
}
