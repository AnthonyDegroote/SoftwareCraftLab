using Cupid.With.Composable;
using Cupid.With.Models;

namespace Cupid.With.Tests.Composable;

/// <summary>
/// Tests des briques composables.
/// CUPID Composable : chaque brique est testée ISOLÉMENT, puis composée.
/// </summary>
public class PricingTests
{
    private static readonly Coffee Espresso = new("Espresso", CoffeeSize.Small, new Money(2.50m));
    private static readonly Coffee Latte = new("Latte", CoffeeSize.Medium, new Money(4.00m));

    [Fact]
    public void WhenSingleLineThenSubTotalIsLineTotal()
    {
        var pricing = new OrderPricing();
        var lines = new[] { new OrderLine(Espresso, 2) };

        var subTotal = pricing.CalculateSubTotal(lines);

        Assert.Equal(5.00m, subTotal.Amount);
    }

    [Fact]
    public void WhenMultipleLinesThenSubTotalIsSumOfLineTotals()
    {
        var pricing = new OrderPricing();
        var lines = new[] { new OrderLine(Espresso, 2), new OrderLine(Latte, 1) };

        var subTotal = pricing.CalculateSubTotal(lines);

        // 2.50×2 + 4.00×1 = 9.00
        Assert.Equal(9.00m, subTotal.Amount);
    }

    [Fact]
    public void WhenEmptyLinesThenSubTotalIsZero()
    {
        var pricing = new OrderPricing();

        var subTotal = pricing.CalculateSubTotal([]);

        Assert.Equal(0m, subTotal.Amount);
    }

    [Fact]
    public void WhenTaxAppliedThenTaxAmountIsCorrect()
    {
        var tax = new TaxCalculation(0.20m);
        var subTotal = new Money(10.00m);

        var taxAmount = tax.CalculateTax(subTotal);

        Assert.Equal(2.00m, taxAmount.Amount);
    }

    [Fact]
    public void WhenDiscountAppliedThenAmountIsReduced()
    {
        var discount = new DiscountCalculation(10); // 10%
        var amount = new Money(100.00m);

        var discounted = discount.ApplyDiscount(amount);

        Assert.Equal(90.00m, discounted.Amount);
    }

    // CUPID Composable : les briques se composent naturellement en pipeline
    [Fact]
    public void WhenComposedInPipelineThenResultIsCorrect()
    {
        var pricing = new OrderPricing();
        var discount = new DiscountCalculation(10);
        var tax = new TaxCalculation(0.20m);

        var lines = new[] { new OrderLine(Latte, 2) }; // 4.00 × 2 = 8.00

        // Pipeline : sous-total → remise → taxe
        var subTotal = pricing.CalculateSubTotal(lines);     // 8.00
        var discounted = discount.ApplyDiscount(subTotal);   // 7.20
        var taxAmount = tax.CalculateTax(discounted);        // 1.44
        var total = discounted + taxAmount;                  // 8.64

        Assert.Equal(8.00m, subTotal.Amount);
        Assert.Equal(7.20m, discounted.Amount);
        Assert.Equal(1.44m, taxAmount.Amount);
        Assert.Equal(8.64m, total.Amount);
    }
}
