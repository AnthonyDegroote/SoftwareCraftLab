using Cupid.Without.Composable;
using Cupid.Without.Models;
using Cupid.Without.Predictable;

namespace Cupid.Without.Tests.Predictable;

/// <summary>
/// Tests du moteur de calcul avec état.
/// ANTI-CUPID Predictable : les tests DOIVENT prendre en compte l'état interne.
/// </summary>
public class PriceCalculationEngineTests
{
    private static OrderProcessingContext CreateContext(decimal price, int qty)
    {
        var order = new OrderDto();
        order.SetCustomerEmail("alice@coffee.com");
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(price);
        item.SetQuantity(qty);
        item.SetSize("Small");
        order.AddItem(item);
        return new OrderProcessingContext { Order = order };
    }

    [Fact]
    public void WhenCalculatePriceThenSubTotalIsSet()
    {
        var engine = new PriceCalculationEngine();
        var context = CreateContext(3.00m, 2);

        engine.Execute(context);

        Assert.Equal(6.00m, context.SubTotal);
    }

    // ANTI-CUPID Predictable : appeler Execute() deux fois DOUBLE le total !
    [Fact]
    public void WhenCalledTwiceThenTotalAccumulates()
    {
        var engine = new PriceCalculationEngine();
        var context = CreateContext(3.00m, 2);

        engine.Execute(context);
        engine.Execute(context);

        // Surprise ! Le total est 12.00 au lieu de 6.00
        Assert.Equal(12.00m, context.SubTotal);
    }

    // ANTI-CUPID Predictable : remise volume cachée, déclenchée automatiquement
    [Fact]
    public void WhenMoreThan5ItemsThenHiddenBulkDiscountApplied()
    {
        var engine = new PriceCalculationEngine();
        var order = new OrderDto();
        order.SetCustomerEmail("alice@coffee.com");
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(10.00m);
        item.SetQuantity(6);
        item.SetSize("Small");
        order.AddItem(item);
        var context = new OrderProcessingContext { Order = order };

        engine.Execute(context);

        // 10 × 6 = 60, mais la remise cachée applique ×0.9 → 54
        Assert.Equal(54.00m, context.SubTotal);
        Assert.True(engine.IsBulkDiscountApplied());
    }

    [Fact]
    public void WhenLessThan5ItemsThenNoBulkDiscount()
    {
        var engine = new PriceCalculationEngine();
        var context = CreateContext(10.00m, 3);

        engine.Execute(context);

        Assert.Equal(30.00m, context.SubTotal);
        Assert.False(engine.IsBulkDiscountApplied());
    }
}
