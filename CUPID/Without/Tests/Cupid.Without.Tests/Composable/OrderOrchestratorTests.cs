using Cupid.Without.Composable;
using Cupid.Without.Domain;
using Cupid.Without.Models;
using Cupid.Without.Predictable;

namespace Cupid.Without.Tests.Composable;

/// <summary>
/// Tests de l'orchestrateur à contexte partagé mutable.
/// ANTI-CUPID Composable : on ne peut pas tester une étape sans le contexte complet.
/// </summary>
public class OrderOrchestratorTests
{
    private static OrderDto CreateOrder(decimal price, int qty)
    {
        var order = new OrderDto();
        order.SetCustomerEmail("alice@coffee.com");
        order.SetCustomerName("Alice");
        var item = new BeverageItemDto();
        item.SetName("Espresso");
        item.SetUnitPrice(price);
        item.SetQuantity(qty);
        item.SetSize("Small");
        order.AddItem(item);
        return order;
    }

    [Fact]
    public void WhenProcessWithPriceStepThenContextContainsSubTotal()
    {
        // Arrange
        // ANTI-CUPID Composable : on doit monter le pipeline complet pour tester le prix
        var steps = new IOrderProcessingStep[] { new PriceCalculationEngine() };
        var orchestrator = new OrderOrchestrator(steps);

        // Act
        var context = orchestrator.Process(CreateOrder(3.00m, 2));

        // Assert
        Assert.Equal(6.00m, context.SubTotal);
    }

    [Fact]
    public void WhenMultipleStepsThenAllMutateSharedContext()
    {
        // Arrange
        var priceEngine = new PriceCalculationEngine();
        var persistence = new EntityPersistenceManager();
        var notification = new NotificationDispatcher();

        var steps = new IOrderProcessingStep[] { priceEngine, persistence, notification };
        var orchestrator = new OrderOrchestrator(steps);

        // Act
        var context = orchestrator.Process(CreateOrder(5.00m, 1));

        // Assert
        // Le contexte mutable a été modifié par chaque étape — couplage implicite
        Assert.Equal(5.00m, context.SubTotal);
        Assert.Single(persistence.DataStore);
        Assert.Single(notification.DispatchedPayloads);
    }

    // ANTI-CUPID Composable : l'ordre des étapes compte car elles partagent un contexte mutable
    [Fact]
    public void WhenNotificationBeforePriceThenTotalIsZeroInNotification()
    {
        // Arrange
        var notification = new NotificationDispatcher();
        var priceEngine = new PriceCalculationEngine();

        // Notification en premier → elle voit FinalTotal = 0 car le prix n'est pas encore calculé
        var steps = new IOrderProcessingStep[] { notification, priceEngine };
        var orchestrator = new OrderOrchestrator(steps);

        // Act
        orchestrator.Process(CreateOrder(5.00m, 1));

        // Assert
        Assert.Contains("amount=0", notification.DispatchedPayloads[0]);
    }
}
