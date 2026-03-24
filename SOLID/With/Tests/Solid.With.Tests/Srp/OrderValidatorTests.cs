using Solid.With.Models;
using Solid.With.Srp;

namespace Solid.With.Tests.Srp;

/// <summary>
/// Tests du OrderValidator (SRP respecté).
/// On peut tester la validation SANS déclencher de calcul, persistance ou notification.
/// Chaque responsabilité est isolée et testable indépendamment.
/// </summary>
public class OrderValidatorTests
{
    private readonly OrderValidator _validator = new();

    private static Order CreateValidOrder() =>
        new("client@example.com", [new OrderItem("Clavier", 50m, 2)]);

    [Fact]
    public void WhenOrderIsValidThenReturnsSuccess()
    {
        // Act
        var result = OrderValidator.Validate(CreateValidOrder());

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void WhenEmailIsMissingThenReturnsFailure()
    {
        // Arrange
        var order = new Order("", [new OrderItem("Clavier", 50m, 1)]);

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("e-mail", result.ErrorMessage);
    }

    [Fact]
    public void WhenNoItemsThenReturnsFailure()
    {
        // Arrange
        var order = new Order("client@example.com", []);

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("au moins un article", result.ErrorMessage);
    }

    [Fact]
    public void WhenQuantityIsZeroThenReturnsFailure()
    {
        // Arrange
        var order = new Order("client@example.com", [new OrderItem("Clavier", 50m, 0)]);

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("quantité", result.ErrorMessage);
    }

    [Fact]
    public void WhenPriceIsNegativeThenReturnsFailure()
    {
        // Arrange
        var order = new Order("client@example.com", [new OrderItem("Clavier", -10m, 1)]);

        // Act
        var result = OrderValidator.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("prix unitaire", result.ErrorMessage);
    }

    [Fact]
    public void WhenOrderIsNullThenThrowsArgumentNull() => Assert.Throws<ArgumentNullException>(() => OrderValidator.Validate(null!));
}
