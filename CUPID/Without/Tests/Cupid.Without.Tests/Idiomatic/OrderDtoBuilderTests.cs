using Cupid.Without.Idiomatic;

namespace Cupid.Without.Tests.Idiomatic;

/// <summary>
/// Tests du builder Java-style.
/// ANTI-CUPID Idiomatic : comparer la verbosité avec un simple constructeur record.
/// </summary>
public class OrderDtoBuilderTests
{
    [Fact]
    public void WhenBuildOrderThenAllPropertiesSet()
    {
        // ANTI-CUPID Idiomatic : 5 lignes pour créer une commande avec 1 article
        // En C# idiomatique : new CoffeeOrder("Alice", "alice@coffee.com", [new(espresso, 2)])
        var order = new OrderDtoBuilder()
            .WithCustomerName("Alice")
            .WithCustomerEmail("alice@coffee.com")
            .AddBeverage("Espresso", 2.50m, 2, "Small")
            .Build();

        Assert.Equal("Alice", order.GetCustomerName());
        Assert.Equal("alice@coffee.com", order.GetCustomerEmail());
        Assert.Single(order.GetItems());
    }

    [Fact]
    public void WhenMultipleBeveragesThenAllAdded()
    {
        var order = new OrderDtoBuilder()
            .WithCustomerName("Bob")
            .WithCustomerEmail("bob@coffee.com")
            .AddBeverage("Espresso", 2.50m, 1, "Small")
            .AddBeverage("Latte", 4.00m, 2, "Medium")
            .Build();

        Assert.Equal(2, order.GetItems().Count);
    }

    // ANTI-CUPID Predictable : Build() renvoie le même objet mutable à chaque appel
    [Fact]
    public void WhenBuildCalledTwiceThenReturnsSameMutableInstance()
    {
        var builder = new OrderDtoBuilder()
            .WithCustomerName("Alice")
            .WithCustomerEmail("alice@coffee.com");

        var order1 = builder.Build();
        var order2 = builder.Build();

        // Surprise ! C'est la même instance mutable
        Assert.Same(order1, order2);
    }
}
