using Cupid.Without.Composable;

namespace Cupid.Without.Predictable;

// SOLID ✓ — SRP (ne fait que le calcul de prix), implémente IOrderProcessingStep (DIP).
// PAS CUPID Predictable ✗ — État interne mutable qui affecte les résultats.
//     Appeler Execute() deux fois donne un résultat DIFFÉRENT (le total s'accumule).
//     Un comportement caché applique automatiquement une remise au-delà de 5 articles.
//     Même entrée ≠ même sortie → non déterministe, imprévisible.

/// <summary>
/// Moteur de calcul de prix avec état interne.
/// </summary>
public class PriceCalculationEngine : IOrderProcessingStep
{
    // ANTI-CUPID Predictable : état caché qui s'accumule entre les appels
    private decimal _runningTotal;
    private int _itemCount;
    private bool _bulkDiscountApplied;

    /// <summary>
    /// Calcule le prix en accumulant dans l'état interne.
    /// Appeler cette méthode deux fois sur le même contexte DOUBLE le total.
    /// </summary>
    public void Execute(OrderProcessingContext context)
    {
        foreach (var item in context.Order.GetItems())
        {
            _runningTotal += item.GetUnitPrice() * item.GetQuantity();
            _itemCount += item.GetQuantity();
        }

        // ANTI-CUPID Predictable : comportement caché — remise automatique au-delà de 5 articles.
        // Rien dans la signature ni le nom de la méthode ne laisse présager cette logique.
        if (_itemCount >= 5 && !_bulkDiscountApplied)
        {
            _runningTotal *= 0.9m;
            _bulkDiscountApplied = true;
        }

        context.SubTotal = _runningTotal;
    }

    /// <summary>
    /// Expose l'état interne — preuve qu'il y a un état caché.
    /// </summary>
    public decimal GetRunningTotal() => _runningTotal;

    /// <summary>
    /// Indique si la remise volume a été appliquée.
    /// </summary>
    public bool IsBulkDiscountApplied() => _bulkDiscountApplied;
}
