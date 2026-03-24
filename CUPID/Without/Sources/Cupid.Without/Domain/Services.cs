using Cupid.Without.Composable;

namespace Cupid.Without.Domain;

// SOLID ✓ — SRP (ne fait que la persistance), DIP (implémente une interface).
// PAS CUPID Domain ✗ — Nommage technique : "EntityPersistenceManager", "PersistEntity".
//     Un barista dit "enregistrer la commande", pas "persister l'entité".
//     Le langage du code ne reflète PAS le langage du métier.

/// <summary>
/// Gère la persistance des entités (nommage technique).
/// </summary>
public class EntityPersistenceManager : IOrderProcessingStep
{
    private readonly List<string> _dataStore = [];

    public IReadOnlyList<string> DataStore => _dataStore;

    /// <summary>
    /// Persiste l'entité dans le magasin de données.
    /// ANTI-CUPID Domain : "PersistEntity" au lieu de "SaveOrder" ou "EnregistrerCommande".
    /// </summary>
    public void Execute(OrderProcessingContext context)
    {
        // ANTI-CUPID Domain : message technique, pas métier
        _dataStore.Add(
            $"Entity persisted: customer={context.Order.GetCustomerEmail()}, total={context.SubTotal}");
    }
}

// SOLID ✓ — SRP (ne fait que la notification), DIP (implémente une interface).
// PAS CUPID Domain ✗ — "NotificationDispatcher", "DispatchPayload" : jargon technique.
//     Le métier dit "envoyer une confirmation", pas "dispatcher une notification".

/// <summary>
/// Dispatche des payloads de notification (nommage technique).
/// </summary>
public class NotificationDispatcher : IOrderProcessingStep
{
    private readonly List<string> _dispatchedPayloads = [];

    public IReadOnlyList<string> DispatchedPayloads => _dispatchedPayloads;

    /// <summary>
    /// Dispatche un payload de notification vers l'endpoint cible.
    /// ANTI-CUPID Domain : "payload", "endpoint", "dispatch" — jargon d'infrastructure.
    /// </summary>
    public void Execute(OrderProcessingContext context)
    {
        _dispatchedPayloads.Add(
            $"Payload dispatched to endpoint: {context.Order.GetCustomerEmail()}, amount={context.FinalTotal}");
    }
}
