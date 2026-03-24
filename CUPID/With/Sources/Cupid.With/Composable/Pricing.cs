using Cupid.With.Models;

namespace Cupid.With.Composable;

// CUPID Composable ✓ — Chaque composant a une petite surface d'API.
//     Entrée → Sortie, sans contexte partagé mutable.
//     Chaque brique est utilisable seule OU composée avec d'autres.
//
// NOTE PÉDAGOGIQUE : CalculateSubTotal() est volontairement identique à
//     PriceCalculator.CalculateSubTotal (namespace Predictable).
//     Les deux classes illustrent des propriétés CUPID DIFFÉRENTES :
//       • Ici (Composable) : petite surface d'API, entrée → sortie, composable en pipeline.
//       • Là-bas (Predictable) : fonction pure, déterministe, sans état caché.
//     La duplication est intentionnelle pour que chaque exemple soit autonome.

/// <summary>
/// Calcule le sous-total d'une commande à partir de ses lignes.
/// Composable : prend des lignes, renvoie un Money — rien d'autre.
/// </summary>
public class OrderPricing
{
    /// <summary>
    /// Calcule le sous-total en additionnant les totaux de chaque ligne.
    /// </summary>
    public Money CalculateSubTotal(IReadOnlyList<OrderLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return lines.Aggregate(Money.Zero, (sum, line) => sum + line.LineTotal);
    }
}

/// <summary>
/// Calcule la taxe sur un montant.
/// Composable : prend un Money, renvoie un Money — composable en pipeline.
/// </summary>
public class TaxCalculation(decimal taxRate)
{
    /// <summary>
    /// Calcule le montant de la taxe.
    /// </summary>
    public Money CalculateTax(Money subTotal)
    {
        ArgumentNullException.ThrowIfNull(subTotal);
        return subTotal * taxRate;
    }
}

/// <summary>
/// Applique une remise en pourcentage.
/// Composable : même signature entrée/sortie que les autres briques.
/// </summary>
public class DiscountCalculation(decimal discountPercent)
{
    /// <summary>
    /// Calcule le montant après remise.
    /// </summary>
    public Money ApplyDiscount(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);
        return amount * (1 - discountPercent / 100m);
    }
}
