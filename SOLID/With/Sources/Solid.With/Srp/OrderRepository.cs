using Solid.With.Models;

namespace Solid.With.Srp;

// PRINCIPE SRP : OrderRepository ne s'occupe QUE de la persistance.
// On peut changer le mode de stockage sans toucher à la validation ni au calcul.

/// <summary>
/// Stocke les commandes traitées (implémentation en mémoire pour la démonstration).
/// </summary>
public class OrderRepository
{
    private readonly List<string> _records = [];

    public IReadOnlyList<string> Records => _records;

    /// <summary>
    /// Sauvegarde le résumé de la commande.
    /// </summary>
    public void Save(Order order, decimal total)
    {
        ArgumentNullException.ThrowIfNull(order);
        _records.Add($"Commande de {order.CustomerEmail} : {total:C}");
    }
}
