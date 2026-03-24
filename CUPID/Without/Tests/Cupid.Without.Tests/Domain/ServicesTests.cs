using Cupid.Without.Composable;
using Cupid.Without.Domain;
using Cupid.Without.Models;

namespace Cupid.Without.Tests.Domain;

/// <summary>
/// Tests des services avec nommage technique.
/// ANTI-CUPID Domain : les noms de tests reflètent le jargon technique, pas le métier.
/// </summary>
public class ServicesTests
{
    private static OrderProcessingContext CreateContextWithSubTotal(decimal subTotal)
    {
        var order = new OrderDto();
        order.SetCustomerEmail("alice@coffee.com");
        return new OrderProcessingContext { Order = order, SubTotal = subTotal };
    }

    [Fact]
    public void WhenEntityPersistedThenDataStoreContainsRecord()
    {
        var manager = new EntityPersistenceManager();
        var context = CreateContextWithSubTotal(10.00m);

        manager.Execute(context);

        // ANTI-CUPID Domain : "Entity persisted" au lieu de "Commande enregistrée"
        Assert.Single(manager.DataStore);
        Assert.Contains("Entity persisted", manager.DataStore[0]);
    }

    [Fact]
    public void WhenPayloadDispatchedThenEndpointReceivesIt()
    {
        var dispatcher = new NotificationDispatcher();
        var context = CreateContextWithSubTotal(10.00m);
        context.FinalTotal = 10.00m;

        dispatcher.Execute(context);

        // ANTI-CUPID Domain : "Payload dispatched to endpoint" au lieu de "Confirmation envoyée"
        Assert.Single(dispatcher.DispatchedPayloads);
        Assert.Contains("Payload dispatched", dispatcher.DispatchedPayloads[0]);
    }
}
