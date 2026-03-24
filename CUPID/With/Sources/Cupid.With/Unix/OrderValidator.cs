using Cupid.With.Models;

namespace Cupid.With.Unix;

// CUPID Unix ✓ — Fait UNE chose, bien, complètement.
//     Pas de framework générique, pas de système de règles configurable.
//     Juste un validateur de commandes de café. Point.
//     Un développeur comprend cette classe en 10 secondes.

/// <summary>
/// Résultat de la validation d'une commande.
/// </summary>
public record OrderValidationResult(bool IsValid, string? Error = null)
{
    public static OrderValidationResult Success() => new(true);
    public static OrderValidationResult Failure(string error) => new(false, error);
}

/// <summary>
/// Valide une commande de café.
/// Unix philosophy : fait une chose, bien, complètement.
/// </summary>
public class OrderValidator
{
    /// <summary>
    /// Valide la commande : client, articles, quantités.
    /// </summary>
    public static OrderValidationResult Validate(CoffeeOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrWhiteSpace(order.CustomerName))
        {
            return OrderValidationResult.Failure("Le nom du client est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            return OrderValidationResult.Failure("L'adresse e-mail du client est obligatoire.");
        }

        if (order.Lines.Count == 0)
        {
            return OrderValidationResult.Failure("La commande doit contenir au moins un café.");
        }

        foreach (var line in order.Lines)
        {
            if (line.Quantity <= 0)
            {
                return OrderValidationResult.Failure($"La quantité de '{line.Coffee.Name}' doit être positive.");
            }
        }

        return OrderValidationResult.Success();
    }
}
