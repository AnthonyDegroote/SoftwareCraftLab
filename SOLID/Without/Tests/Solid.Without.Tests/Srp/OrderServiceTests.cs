using Solid.Without.Models;
using Solid.Without.Srp;

namespace Solid.Without.Tests.Srp;

/// <summary>
/// Tests du OrderService (God class — violation SRP).
/// Remarque : tester cette classe est difficile car chaque test déclenche
/// la validation, le calcul, la persistance ET la notification.
/// On ne peut pas tester une responsabilité isolément.
/// </summary>
public class OrderServiceTests
{
    private readonly OrderService _service = new();

    private static Order CreateValidOrder(string? discountCode = null) =>
        new("client@example.com",
            [new OrderItem("Clavier", 50m, 2), new OrderItem("Souris", 25m, 1)],
            discountCode);

    [Fact]
    public void WhenOrderIsValidThenReturnsTotalPrice()
    {
        // Act
        decimal total = _service.ProcessOrder(CreateValidOrder());

        // Assert
        // 50×2 + 25×1 = 125
        Assert.Equal(125m, total);
    }

    [Fact]
    public void WhenDiscountPercent10ThenApplies10PercentOff()
    {
        // Act
        decimal total = _service.ProcessOrder(CreateValidOrder("PERCENT10"));

        // Assert
        // 125 × 0.90 = 112.5
        Assert.Equal(112.5m, total);
    }

    [Fact]
    public void WhenDiscountFixed5ThenSubtracts5()
    {
        // Act
        decimal total = _service.ProcessOrder(CreateValidOrder("FIXED5"));

        // Assert
        // 125 - 5 = 120
        Assert.Equal(120m, total);
    }

    [Fact]
    public void WhenUnknownDiscountCodeThenNoDiscountApplied()
    {
        // Act
        decimal total = _service.ProcessOrder(CreateValidOrder("UNKNOWN"));

        // Assert
        Assert.Equal(125m, total);
    }

    [Fact]
    public void WhenOrderHasNoItemsThenThrows()
    {
        // Arrange
        var order = new Order("client@example.com", []);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => _service.ProcessOrder(order));

        // Assert
        Assert.Contains("au moins un article", exception.Message);
    }

    [Fact]
    public void WhenEmailIsMissingThenThrows()
    {
        // Arrange
        var order = new Order("", [new OrderItem("Clavier", 50m, 1)]);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => _service.ProcessOrder(order));

        // Assert
        Assert.Contains("e-mail", exception.Message);
    }

    [Fact]
    public void WhenQuantityIsZeroThenThrows()
    {
        // Arrange
        var order = new Order("client@example.com", [new OrderItem("Clavier", 50m, 0)]);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => _service.ProcessOrder(order));

        // Assert
        Assert.Contains("quantité", exception.Message);
    }

    // ANTI-PATTERN : On ne peut pas tester la persistance SANS aussi valider et calculer.
    [Fact]
    public void WhenOrderProcessedThenOrderIsSaved()
    {
        // Act
        _service.ProcessOrder(CreateValidOrder());

        // Assert
        Assert.Single(_service.SavedOrders);
    }

    // ANTI-PATTERN : On ne peut pas tester la notification SANS aussi valider, calculer et persister.
    [Fact]
    public void WhenOrderProcessedThenEmailIsSent()
    {
        // Act
        _service.ProcessOrder(CreateValidOrder());

        // Assert
        Assert.Single(_service.SentEmails);
    }
}
