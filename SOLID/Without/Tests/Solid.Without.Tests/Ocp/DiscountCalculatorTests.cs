using Solid.Without.Ocp;

namespace Solid.Without.Tests.Ocp;

/// <summary>
/// Tests du DiscountCalculator (violation OCP).
/// Les tests couvrent les cas existants, mais chaque nouveau type de remise
/// nécessite de modifier le code source ET d'ajouter de nouveaux tests.
///
/// ANTI-PATTERN OCP : le switch fermé oblige à passer un string "discountType".
/// [Theory] + [InlineData] permet de vérifier tous les cas de manière concise,
/// mais on voit bien que chaque nouveau type de remise impose de modifier le switch.
/// </summary>
public class DiscountCalculatorTests
{
    private readonly DiscountCalculator _calculator = new();

    [Theory]
    [InlineData(100, "Percentage", 10, 90)]       // 10 % sur 100 → 90
    [InlineData(100, "FixedAmount", 15, 85)]       // Remise fixe 15 sur 100 → 85
    [InlineData(150, "BuyTwoGetOneFree", 0, 100)]  // 2+1 gratuit sur 150 → 100
    [InlineData(100, "LoyaltyPoints", 50, 100)]    // Type inconnu → aucune remise
    [InlineData(10, "FixedAmount", 20, 0)]         // Remise fixe dépasse le total → 0
    public void WhenApplyDiscountThenReturnsExpectedAmount(
        decimal amount, string discountType, decimal discountValue, decimal expected)
    {
        decimal result = DiscountCalculator.ApplyDiscount(amount, discountType, discountValue);

        Assert.Equal(expected, result);
    }
}
