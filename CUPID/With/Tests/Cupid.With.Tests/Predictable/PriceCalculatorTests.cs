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

    private static readonly Coffee Espresso = new("Espresso", CoffeeSize.Small, new Money(2.50m));
    private static readonly Coffee Latte = new("Latte", CoffeeSize.Medium, new Money(4.00m));

    [Fact]
    public void WhenCalculateSubTotalThenSumsAllLines()
    {
        var lines = new[] { new OrderLine(Espresso, 2), new OrderLine(Latte, 1) };

        var subTotal = _calculator.CalculateSubTotal(lines);

        // 2.50×2 + 4.00×1 = 9.00
        Assert.Equal(9.00m, subTotal.Amount);
    }

    // CUPID Predictable : appeler 10 fois donne 10 fois le même résultat (pas d'état)
    [Fact]
    public void WhenCalledMultipleTimesThenSameResult()
    {
        var lines = new[] { new OrderLine(Espresso, 2) };

        var result1 = _calculator.CalculateSubTotal(lines);
        var result2 = _calculator.CalculateSubTotal(lines);
        var result3 = _calculator.CalculateSubTotal(lines);

        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
    }

    [Fact]
    public void WhenCalculateTaxThenAmountIsCorrect()
    {
        var subTotal = new Money(10.00m);

        var tax = _calculator.CalculateTax(subTotal, 0.20m);

        Assert.Equal(2.00m, tax.Amount);
    }

    [Fact]
    public void WhenCalculateTotalThenSubTotalPlusTax()
    {
        var subTotal = new Money(10.00m);
        var tax = new Money(2.00m);

        var total = _calculator.CalculateTotal(subTotal, tax);

        Assert.Equal(12.00m, total.Amount);
    }

    // CUPID Predictable : pas de remise cachée, pas de surprise
    [Fact]
    public void WhenLargeOrderThenNoHiddenDiscount()
    {
        var lines = new[] { new OrderLine(Espresso, 100) };

        var subTotal = _calculator.CalculateSubTotal(lines);

        // 2.50 × 100 = 250.00, pas de remise cachée
        Assert.Equal(250.00m, subTotal.Amount);
    }
}
