using Cupid.With.Models;
using Cupid.With.Predictable;

namespace Cupid.With.Tests.Predictable;

/// <summary>
/// Tests du calculateur de prix pur.
/// CUPID Predictable : même entrée → même sortie, toujours.
/// </summary>
public class PriceCalculatorTests
{
    private readonly PriceCalculator _calculator = new();

    private static readonly Coffee _espresso = new("Espresso", CoffeeSize.Small, new Money(2.50m));
    private static readonly Coffee _latte = new("Latte", CoffeeSize.Medium, new Money(4.00m));

    [Fact]
    public void WhenCalculateSubTotalThenSumsAllLines()
    {
        // Arrange
        var lines = new[] { new OrderLine(_espresso, 2), new OrderLine(_latte, 1) };

        // Act
        var subTotal = PriceCalculator.CalculateSubTotal(lines);

        // Assert
        // 2.50×2 + 4.00×1 = 9.00
        Assert.Equal(9.00m, subTotal.Amount);
    }

    // CUPID Predictable : appeler 10 fois donne 10 fois le même résultat (pas d'état)
    [Fact]
    public void WhenCalledMultipleTimesThenSameResult()
    {
        // Arrange
        var lines = new[] { new OrderLine(_espresso, 2) };

        // Act
        var result1 = PriceCalculator.CalculateSubTotal(lines);
        var result2 = PriceCalculator.CalculateSubTotal(lines);
        var result3 = PriceCalculator.CalculateSubTotal(lines);

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
    }

    [Fact]
    public void WhenCalculateTaxThenAmountIsCorrect()
    {
        // Arrange
        var subTotal = new Money(10.00m);

        // Act
        var tax = PriceCalculator.CalculateTax(subTotal, 0.20m);

        // Assert
        Assert.Equal(2.00m, tax.Amount);
    }

    [Fact]
    public void WhenCalculateTotalThenSubTotalPlusTax()
    {
        // Arrange
        var subTotal = new Money(10.00m);
        var tax = new Money(2.00m);

        // Act
        var total = PriceCalculator.CalculateTotal(subTotal, tax);

        // Assert
        Assert.Equal(12.00m, total.Amount);
    }

    // CUPID Predictable : pas de remise cachée, pas de surprise
    [Fact]
    public void WhenLargeOrderThenNoHiddenDiscount()
    {
        // Arrange
        var lines = new[] { new OrderLine(_espresso, 100) };

        // Act
        var subTotal = PriceCalculator.CalculateSubTotal(lines);

        // Assert
        // 2.50 × 100 = 250.00, pas de remise cachée
        Assert.Equal(250.00m, subTotal.Amount);
    }
}
