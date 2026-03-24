namespace Cupid.With.Models;

// CUPID Domain ✓ — Money est un Value Object métier, pas un simple decimal.
//     Il capture la sémantique du domaine : un montant A une devise.
// CUPID Idiomatic ✓ — Record immuable, surcharge d'opérateurs C# naturelle.
// CUPID Predictable ✓ — Immuable, toute opération renvoie un nouvel objet.

/// <summary>
/// Valeur monétaire immuable — Value Object du domaine.
/// </summary>
public record Money(decimal Amount, string Currency = "EUR")
{
    /// <summary>
    /// Montant nul dans la devise par défaut.
    /// </summary>
    public static readonly Money Zero = new(0);

    // CUPID Idiomatic : opérateurs C# pour une API naturelle (total + prix, prix * quantité)
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException(
                $"Impossible d'additionner {left.Currency} et {right.Currency}.");
        }

        return left with { Amount = left.Amount + right.Amount };
    }

    public static Money operator *(Money money, decimal factor) =>
        money with { Amount = money.Amount * factor };

    public static Money operator *(decimal factor, Money money) =>
        money * factor;
}
