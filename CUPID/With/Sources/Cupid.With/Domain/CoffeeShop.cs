using Cupid.With.Composable;
using Cupid.With.Models;
using Cupid.With.Unix;

namespace Cupid.With.Domain;

// CUPID Domain ✓ — Le code utilise le langage ubiquitaire du coffee shop.
//     "PlaceOrder" (pas "ProcessEntity"), "OrderConfirmation" (pas "ResultDto"),
//     "CoffeeShop" (pas "OrderOrchestrator"), "SaveOrder" (pas "PersistEntity").
//     Un barista qui lit ce code comprend les opérations métier.

/// <summary>
/// Abstraction pour la sauvegarde des commandes.
/// Domain : "SaveOrder" est le verbe métier.
/// </summary>
public interface IOrderStore
{
    /// <summary>
    /// Sauvegarde une commande confirmée.
    /// </summary>
    void SaveOrder(OrderConfirmation confirmation);
}

/// <summary>
/// Abstraction pour l'envoi de confirmations.
/// Domain : "SendConfirmation" parle le langage du client.
/// </summary>
public interface IConfirmationNotifier
{
    /// <summary>
    /// Envoie une confirmation au client.
    /// </summary>
    void SendConfirmation(OrderConfirmation confirmation);
}

/// <summary>
/// Façade métier du coffee shop.
/// Domain : le nom de la classe EST le domaine.
/// Composable : dépend de briques simples, composées ici.
/// </summary>
public class CoffeeShop(
    OrderValidator validator,
    OrderPricing pricing,
    TaxCalculation tax,
    IOrderStore store,
    IConfirmationNotifier notifier)
{
    /// <summary>
    /// Passe une commande de café.
    /// Domain : "PlaceOrder" est l'opération métier, pas "ProcessEntity".
    /// </summary>
    public OrderConfirmation PlaceOrder(CoffeeOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var validation = validator.Validate(order);
        if (!validation.IsValid)
        {
            throw new InvalidOperationException(validation.Error);
        }

        // CUPID Composable : chaque brique est appelée en séquence,
        // chacune prend l'output de la précédente — pas de contexte mutable partagé.
        var subTotal = pricing.CalculateSubTotal(order.Lines);
        var taxAmount = tax.CalculateTax(subTotal);
        var total = subTotal + taxAmount;

        var confirmation = new OrderConfirmation(order, subTotal, taxAmount, total);

        store.SaveOrder(confirmation);
        notifier.SendConfirmation(confirmation);

        return confirmation;
    }
}

/// <summary>
/// Sauvegarde en mémoire (pour les tests et la démonstration).
/// </summary>
public class InMemoryOrderStore : IOrderStore
{
    private readonly List<OrderConfirmation> _orders = [];
    public IReadOnlyList<OrderConfirmation> Orders => _orders;

    public void SaveOrder(OrderConfirmation confirmation) => _orders.Add(confirmation);
}

/// <summary>
/// Notification en mémoire (pour les tests et la démonstration).
/// </summary>
public class InMemoryConfirmationNotifier : IConfirmationNotifier
{
    private readonly List<OrderConfirmation> _sent = [];
    public IReadOnlyList<OrderConfirmation> SentConfirmations => _sent;

    public void SendConfirmation(OrderConfirmation confirmation) => _sent.Add(confirmation);
}
