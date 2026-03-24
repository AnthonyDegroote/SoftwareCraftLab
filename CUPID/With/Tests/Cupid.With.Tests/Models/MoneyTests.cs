using Cupid.With.Models;

namespace Cupid.With.Tests.Models;

/// <summary>
/// Tests du Value Object Money.
/// CUPID Predictable : immuable, opérations pures, résultat toujours prévisible.
/// CUPID Idiomatic : les opérateurs rendent le code naturel.
/// </summary>
public class MoneyTests
{
    [Fact]
    public void WhenAddTwoMoneysThenSumIsCorrect()
    {
        // Arrange
        var a = new Money(3.50m);
        var b = new Money(2.00m);

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(5.50m, result.Amount);
    }

    [Fact]
    public void WhenMultiplyByFactorThenProductIsCorrect()
    {
        // Arrange
        var price = new Money(4.00m);

        // Act
        var result = price * 3;

        // Assert
        Assert.Equal(12.00m, result.Amount);
    }

    [Fact]
    public void WhenAddDifferentCurrenciesThenThrows()
    {
        // Arrange
        var euros = new Money(10.00m, "EUR");
        var dollars = new Money(5.00m, "USD");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => euros + dollars);
    }

    // CUPID Predictable : Money.Zero est toujours le même, immuable
    [Fact]
    public void WhenAddZeroThenOriginalUnchanged()
    {
        // Arrange
        var price = new Money(5.00m);

        // Act
        var result = price + Money.Zero;

        // Assert
        Assert.Equal(5.00m, result.Amount);
    }

    // CUPID Idiomatic : les records C# offrent l'égalité structurelle gratuitement
    [Fact]
    public void WhenSameAmountAndCurrencyThenEqual()
    {
        // Arrange
        var a = new Money(3.50m, "EUR");
        var b = new Money(3.50m, "EUR");

        // Assert
        Assert.Equal(a, b);
    }
}
