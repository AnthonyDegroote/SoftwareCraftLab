using Solid.With.Models;

namespace Solid.With.Srp;

// PRINCIPE SRP : Chaque classe a une seule raison de changer.
// OrderValidator ne s'occupe QUE de la validation des commandes.
// Si les règles de validation évoluent, seule cette classe est impactée.

/// <summary>
/// Valide la conformité d'une commande avant traitement.
/// </summary>
public class OrderValidator
{
    /// <summary>
    /// Vérifie que la commande est valide (articles, e-mail, quantités, prix).
    /// </summary>
    public static OrderValidationResult Validate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            return OrderValidationResult.Failure("L'adresse e-mail du client est obligatoire.");
        }

        if (order.Items.Count == 0)
        {
            return OrderValidationResult.Failure("La commande doit contenir au moins un article.");
        }

        foreach (var item in order.Items)
        {
            if (item.Quantity <= 0)
            {
                return OrderValidationResult.Failure(
                    $"La quantité de '{item.ProductName}' doit être positive.");
            }

            if (item.UnitPrice < 0)
            {
                return OrderValidationResult.Failure(
                    $"Le prix unitaire de '{item.ProductName}' ne peut pas être négatif.");
            }
        }

        return OrderValidationResult.Success();
    }
}

/// <summary>
/// Résultat de la validation d'une commande.
/// </summary>
public record OrderValidationResult(bool IsValid, string? ErrorMessage = null)
{
    public static OrderValidationResult Success() => new(true);
    public static OrderValidationResult Failure(string message) => new(false, message);
}
