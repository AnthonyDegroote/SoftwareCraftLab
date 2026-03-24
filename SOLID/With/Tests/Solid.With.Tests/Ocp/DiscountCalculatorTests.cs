using Solid.With.Ocp;

namespace Solid.With.Tests.Ocp;

/// <summary>
/// Tests du DiscountCalculator et des stratégies de remise (OCP respecté).
/// Ajouter une nouvelle stratégie = ajouter une nouvelle classe + ses tests,
/// SANS modifier le code existant.
/// </summary>
public class DiscountCalculatorTests
{
    private readonly DiscountCalculator _calculator = new();

    [Fact]
    public void WhenPercentageDiscountThenReducesByPercentage()
    {
        var strategy = new PercentageDiscount(10);

        decimal result = _calculator.ApplyDiscount(100m, strategy);

        Assert.Equal(90m, result);
    }

    // [Theory] + [InlineData] : même stratégie (FixedAmountDiscount), plusieurs scénarios de montant.
    [Theory]
    [InlineData(100, 15, 85)]   // Remise fixe 15 sur 100 → 85
    [InlineData(100, 200, 0)]   // Remise fixe dépasse le total → 0
    public void WhenFixedAmountDiscountThenReturnsExpectedAmount(
        decimal amount, decimal discountAmount, decimal expected)
    {
        var strategy = new FixedAmountDiscount(discountAmount);

        decimal result = _calculator.ApplyDiscount(amount, strategy);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenNoDiscountThenReturnsOriginalAmount()
    {
        var strategy = new NoDiscount();

        decimal result = _calculator.ApplyDiscount(100m, strategy);

        Assert.Equal(100m, result);
    }

    // PRINCIPE OCP : on peut ajouter une nouvelle stratégie sans modifier le calculateur.
    // Exemple : remise "2 pour le prix de 1" ajoutée UNIQUEMENT ici.
    [Fact]
    public void WhenCustomStrategyThenCalculatorAppliesIt()
    {
        var strategy = new BuyTwoGetOneFreeDiscount();

        decimal result = _calculator.ApplyDiscount(150m, strategy);

        Assert.Equal(100m, result);
    }

    /// <summary>
    /// Nouvelle stratégie créée pour le test — aucun code existant modifié.
    /// </summary>
    private sealed class BuyTwoGetOneFreeDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal amount) => amount * 2m / 3m;
    }
}
