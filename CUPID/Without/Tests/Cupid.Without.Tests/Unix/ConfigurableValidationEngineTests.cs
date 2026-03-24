using Cupid.Without.Models;
using Cupid.Without.Unix;

namespace Cupid.Without.Tests.Unix;

/// <summary>
/// Tests du moteur de validation générique.
/// ANTI-CUPID Unix : chaque test nécessite de CONFIGURER le moteur avant usage.
/// Un développeur doit comprendre le système de règles pour valider une commande.
/// </summary>
public class ConfigurableValidationEngineTests
{
    // ANTI-CUPID Unix : boilerplate nécessaire pour configurer le moteur
    private static ConfigurableValidationEngine<OrderDto> CreateOrderValidator() =>
        new ConfigurableValidationEngine<OrderDto>()
            .AddRule(
                o => !string.IsNullOrWhiteSpace(o.GetCustomerEmail()),
                "L'adresse e-mail est obligatoire.")
            .AddRule(
                o => o.GetItems().Count > 0,
                "La commande doit contenir au moins un article.")
            .AddRule(
                o => o.GetItems().TrueForAll(i => i.GetQuantity() > 0),
                "Toutes les quantités doivent être positives.");

    [Fact]
    public void WhenValidOrderThenReturnsSuccess()
    {
        // Arrange
        var engine = CreateOrderValidator();
        var order = new OrderDto();
        order.SetCustomerEmail("alice@coffee.com");
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");
        order.AddItem(item);

        // Act
        var result = engine.Validate(order);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void WhenMissingEmailThenReturnsError()
    {
        // Arrange
        var engine = CreateOrderValidator();
        var order = new OrderDto();
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(2.50m);
        item.SetQuantity(1);
        item.SetSize("Small");
        order.AddItem(item);

        // Act
        var result = engine.Validate(order);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("e-mail", result.Errors[0]);
    }

    [Fact]
    public void WhenStopOnFirstErrorDisabledThenReturnsAllErrors()
    {
        // Arrange
        var engine = CreateOrderValidator().WithStopOnFirstError(false);
        var order = new OrderDto(); // Pas d'email, pas d'articles

        // Act
        var result = engine.Validate(order);

        // Assert
        Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public void WhenErrorCallbackConfiguredThenCallbackInvoked()
    {
        // Arrange
        var callbackMessages = new List<string>();
        var engine = CreateOrderValidator()
            .WithErrorCallback(msg => callbackMessages.Add(msg));
        var order = new OrderDto();

        // Act
        engine.Validate(order);

        // Assert
        Assert.Single(callbackMessages);
    }
}
