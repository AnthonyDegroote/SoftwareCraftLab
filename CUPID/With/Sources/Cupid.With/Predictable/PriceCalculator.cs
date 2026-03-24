using Cupid.With.Models;

namespace Cupid.With.Predictable;

// CUPID Predictable ✓ — Fonctions pures, aucun état interne.
//     Même entrée → même sortie, toujours, sans exception.
//     Pas de compteur caché, pas de remise automatique surprise.
//     Le comportement est entièrement déterminé par les paramètres.
//
// NOTE PÉDAGOGIQUE : CalculateSubTotal() est volontairement identique à
//     OrderPricing.CalculateSubTotal (namespace Composable).
//     Les deux classes illustrent des propriétés CUPID DIFFÉRENTES :
//       • Ici (Predictable) : fonction pure, déterministe, sans état caché.
//       • Là-bas (Composable) : petite surface d'API, entrée → sortie, composable en pipeline.
//     La duplication est intentionnelle pour que chaque exemple soit autonome.

/// <summary>
/// Calcule le prix d'une commande. Fonction pure, sans état.
/// </summary>
public class PriceCalculator
{
    /// <summary>
    /// Calcule le sous-total. Appeler 10 fois = 10 fois le même résultat.
    /// </summary>
    public Money CalculateSubTotal(IReadOnlyList<OrderLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return lines.Aggregate(Money.Zero, (sum, line) => sum + line.LineTotal);
    }

    /// <summary>
    /// Calcule la taxe à partir du sous-total et du taux.
    /// Tous les paramètres sont explicites — pas de magie cachée.
    /// </summary>
    public Money CalculateTax(Money subTotal, decimal taxRate)
    {
        ArgumentNullException.ThrowIfNull(subTotal);
        return subTotal * taxRate;
    }

    /// <summary>
    /// Calcule le total final (sous-total + taxe).
    /// </summary>
    public Money CalculateTotal(Money subTotal, Money tax) => subTotal + tax;
}
