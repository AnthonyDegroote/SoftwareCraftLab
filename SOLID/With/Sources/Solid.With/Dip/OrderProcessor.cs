using Solid.With.Models;

namespace Solid.With.Dip;

// PRINCIPE DIP : Inversion des dépendances
// Le module de haut niveau (OrderProcessor) dépend d'ABSTRACTIONS (interfaces),
// pas de classes concrètes. Les détails d'implémentation sont injectés de l'extérieur.
// Avantages :
//   - Testable unitairement avec des mocks/stubs
//   - Changement de persistance ou de notification sans toucher à OrderProcessor
//   - Couplage faible entre modules

/// <summary>
/// Abstraction pour la persistance des commandes.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Sauvegarde une commande traitée.
    /// </summary>
    void Save(Order order, decimal total);
}

/// <summary>
/// Abstraction pour l'envoi de notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Envoie une confirmation au client.
    /// </summary>
    void SendConfirmation(string email, decimal total);
}

// PRINCIPE DIP : OrderProcessor reçoit ses dépendances par injection
// de constructeur. Il ne connaît QUE les interfaces, pas les implémentations.

/// <summary>
/// Traite une commande en déléguant la persistance et la notification
/// à des abstractions injectées.
/// </summary>
public class OrderProcessor(IOrderRepository repository, INotificationService notificationService)
{
    /// <summary>
    /// Traite la commande : calcule le total, sauvegarde et notifie.
    /// </summary>
    public string Process(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        decimal total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        repository.Save(order, total);
        notificationService.SendConfirmation(order.CustomerEmail, total);

        return $"Commande traitée : {total:C}";
    }
}

// Implémentations concrètes — interchangeables sans modifier OrderProcessor.

/// <summary>
/// Persistance en mémoire (pour les tests et la démonstration).
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<string> _records = [];
    public IReadOnlyList<string> Records => _records;

    public void Save(Order order, decimal total) => _records.Add($"Commande de {order.CustomerEmail} : {total:C}");
}

/// <summary>
/// Notification en mémoire (pour les tests et la démonstration).
/// </summary>
public class InMemoryNotificationService : INotificationService
{
    private readonly List<string> _sentMessages = [];
    public IReadOnlyList<string> SentMessages => _sentMessages;

    public void SendConfirmation(string email, decimal total) => _sentMessages.Add($"Confirmation envoyée à {email} pour {total:C}");
}
