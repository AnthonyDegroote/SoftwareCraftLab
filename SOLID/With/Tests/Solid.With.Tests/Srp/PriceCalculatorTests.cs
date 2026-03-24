using Solid.With.Models;
using Solid.With.Srp;

namespace Solid.With.Tests.Srp;

/// <summary>
/// Tests du PriceCalculator (SRP respecté).
/// On teste le calcul du prix SANS validation ni persistance.
/// </summary>
public class PriceCalculatorTests
{
    private readonly PriceCalculator _calculator = new();

    [Fact]
    public void WhenSingleItemThenReturnsPriceTimesQuantity()
    {
        var order = new Order("client@example.com", [new OrderItem("Clavier", 50m, 3)]);

        decimal total = PriceCalculator.CalculateTotal(order);

        Assert.Equal(150m, total);
    }

    [Fact]
    public void WhenMultipleItemsThenReturnsSumOfAllItems()
    {
        var order = new Order("client@example.com",
            [new OrderItem("Clavier", 50m, 2), new OrderItem("Souris", 25m, 1)]);

        decimal total = PriceCalculator.CalculateTotal(order);

        // 50×2 + 25×1 = 125
        Assert.Equal(125m, total);
    }

    [Fact]
    public void WhenOrderIsNullThenThrowsArgumentNull() => Assert.Throws<ArgumentNullException>(() => PriceCalculator.CalculateTotal(null!));
}
