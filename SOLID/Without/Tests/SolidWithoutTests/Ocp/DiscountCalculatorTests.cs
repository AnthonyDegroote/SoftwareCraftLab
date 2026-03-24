using Solid.Without.Ocp;

namespace Solid.Without.Tests.Ocp;

/// <summary>
/// Tests du DiscountCalculator (violation OCP).
/// Les tests couvrent les cas existants, mais chaque nouveau type de remise
/// nécessite de modifier le code source ET d'ajouter de nouveaux tests.
/// </summary>
public class DiscountCalculatorTests
{
    private readonly DiscountCalculator _calculator = new();

    [Fact]
    public void WhenPercentageDiscountThenReducesByPercentage()
    {
        decimal result = _calculator.ApplyDiscount(100m, "Percentage", 10m);

        Assert.Equal(90m, result);
    }

    [Fact]
    public void WhenFixedAmountDiscountThenSubtractsAmount()
    {
        decimal result = _calculator.ApplyDiscount(100m, "FixedAmount", 15m);

        Assert.Equal(85m, result);
    }

    [Fact]
    public void WhenBuyTwoGetOneFreeThenReducesByOneThird()
    {
        decimal result = _calculator.ApplyDiscount(150m, "BuyTwoGetOneFree", 0m);

        Assert.Equal(100m, result);
    }

    [Fact]
    public void WhenUnknownDiscountTypeThenNoDiscount()
    {
        decimal result = _calculator.ApplyDiscount(100m, "LoyaltyPoints", 50m);

        // ANTI-PATTERN : "LoyaltyPoints" n'existe pas dans le switch → silencieusement ignoré
        Assert.Equal(100m, result);
    }

    [Fact]
    public void WhenFixedAmountExceedsTotalThenReturnsZero()
    {
        decimal result = _calculator.ApplyDiscount(10m, "FixedAmount", 20m);

        Assert.Equal(0m, result);
    }
}
