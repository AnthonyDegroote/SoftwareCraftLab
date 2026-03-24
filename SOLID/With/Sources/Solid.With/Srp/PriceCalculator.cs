using Solid.With.Models;

namespace Solid.With.Srp;

// PRINCIPE SRP : PriceCalculator ne s'occupe QUE du calcul du prix.
// Si la logique de tarification change, seule cette classe est impactée.

/// <summary>
/// Calcule le total d'une commande à partir de ses articles.
/// </summary>
public class PriceCalculator
{
    /// <summary>
    /// Calcule le prix total brut de la commande.
    /// </summary>
    public decimal CalculateTotal(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Items.Sum(item => item.UnitPrice * item.Quantity);
    }
}
