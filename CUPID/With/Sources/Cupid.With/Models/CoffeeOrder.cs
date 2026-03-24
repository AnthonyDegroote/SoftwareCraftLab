namespace Cupid.With.Models;

// CUPID Domain ✓ — Les types parlent le métier : Coffee, OrderLine, CoffeeOrder.
//     Un barista reconnaîtrait ces concepts immédiatement.
// CUPID Idiomatic ✓ — Records C# immuables avec propriétés calculées.
// CUPID Predictable ✓ — Immutabilité totale, pas d'effet de bord.

/// <summary>
/// Un café avec son nom, sa taille et son prix.
/// </summary>
public record Coffee(string Name, CoffeeSize Size, Money Price);

/// <summary>
/// Une ligne de commande : un café × une quantité.
/// Le total de la ligne est calculé automatiquement (propriété dérivée).
/// </summary>
public record OrderLine(Coffee Coffee, int Quantity)
{
    // CUPID Predictable : propriété calculée pure, pas d'état caché.
    public Money LineTotal => Coffee.Price * Quantity;
}

/// <summary>
/// Une commande de café complète, immuable.
/// </summary>
public record CoffeeOrder(
    string CustomerName,
    string CustomerEmail,
    IReadOnlyList<OrderLine> Lines);

/// <summary>
/// Confirmation de commande produite après traitement.
/// </summary>
public record OrderConfirmation(
    CoffeeOrder Order,
    Money SubTotal,
    Money Tax,
    Money Total);
