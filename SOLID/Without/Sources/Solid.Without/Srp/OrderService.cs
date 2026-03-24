using Solid.Without.Models;

namespace Solid.Without.Srp;

// ANTI-PATTERN : Violation du principe de responsabilité unique (SRP)
// Cette classe cumule TOUTES les responsabilités :
//   - Validation de la commande
//   - Calcul du prix total et des remises
//   - Persistance (écriture sur disque)
//   - Notification (envoi d'e-mail simulé)
// Conséquences : difficile à tester unitairement, toute modification risque d'impacter
// des fonctionnalités non liées, couplage fort entre les responsabilités.
public class OrderService
{
    private readonly List<string> _savedOrders = [];
    private readonly List<string> _sentEmails = [];

    // ANTI-PATTERN : Les listes internes sont exposées en lecture seule,
    // mais l'encapsulation reste faible car toute la logique est dans cette classe.
    public IReadOnlyList<string> SavedOrders => _savedOrders;
    public IReadOnlyList<string> SentEmails => _sentEmails;

    /// <summary>
    /// Traite une commande de bout en bout :
    /// valide, calcule, persiste et notifie — tout dans la même méthode.
    /// </summary>
    public decimal ProcessOrder(Order order)
    {
        // --- Responsabilité 1 : Validation ---
        // ANTI-PATTERN : La logique de validation est mélangée au traitement
        if (order.Items.Count == 0)
        {
            throw new InvalidOperationException("La commande doit contenir au moins un article.");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            throw new InvalidOperationException("L'adresse e-mail du client est obligatoire.");
        }

        foreach (var item in order.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException($"La quantité de '{item.ProductName}' doit être positive.");
            }

            if (item.UnitPrice < 0)
            {
                throw new InvalidOperationException($"Le prix unitaire de '{item.ProductName}' ne peut pas être négatif.");
            }
        }

        // --- Responsabilité 2 : Calcul du prix ---
        // ANTI-PATTERN : Le calcul est enfoui dans la même méthode
        decimal total = 0;
        foreach (var item in order.Items)
        {
            total += item.UnitPrice * item.Quantity;
        }

        // ANTI-PATTERN : La logique de remise utilise un switch/case fermé (viole aussi OCP)
        if (!string.IsNullOrEmpty(order.DiscountCode))
        {
            total = order.DiscountCode switch
            {
                "PERCENT10" => total * 0.90m,
                "PERCENT20" => total * 0.80m,
                "FIXED5" => total - 5m,
                "FIXED10" => total - 10m,
                _ => total
            };
        }

        if (total < 0)
        {
            total = 0;
        }

        // --- Responsabilité 3 : Persistance ---
        // ANTI-PATTERN : La sauvegarde est directement dans le service métier
        _savedOrders.Add($"Commande pour {order.CustomerEmail} : {total:C}");

        // --- Responsabilité 4 : Notification ---
        // ANTI-PATTERN : L'envoi de notification est couplé au traitement
        _sentEmails.Add($"Confirmation envoyée à {order.CustomerEmail} pour un total de {total:C}");

        return total;
    }
}
