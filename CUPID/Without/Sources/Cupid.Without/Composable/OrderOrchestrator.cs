using Cupid.Without.Models;

namespace Cupid.Without.Composable;

// SOLID ✓ — DIP (interface), OCP (étapes extensibles), SRP (orchestration seule).
// PAS CUPID Composable ✗ — Les composants communiquent via un contexte mutable partagé.
//     Chaque étape modifie le même objet → impossible d'utiliser un composant isolément.
//     Le résultat d'une étape dépend de ce que les étapes précédentes ont muté.

/// <summary>
/// Contexte partagé mutable — véhicule d'état entre toutes les étapes du pipeline.
/// </summary>
public class OrderProcessingContext
{
    public required OrderDto Order { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal FinalTotal { get; set; }
    public bool IsValid { get; set; }
    public List<string> Errors { get; } = [];
    public List<string> Logs { get; } = [];
}

/// <summary>
/// Abstraction d'une étape de traitement.
/// SOLID ✓ — ISP : interface minimale.
/// PAS CUPID Composable ✗ — L'étape ne prend pas une entrée et ne renvoie pas une sortie.
///     Elle mute un contexte partagé, ce qui la rend inutilisable seule.
/// </summary>
public interface IOrderProcessingStep
{
    void Execute(OrderProcessingContext context);
}

/// <summary>
/// Orchestre les étapes de traitement dans l'ordre.
/// SOLID ✓ — OCP (nouvelles étapes ajoutables sans modification), DIP (dépend de l'abstraction).
/// PAS CUPID Composable ✗ — La valeur ajoutée de chaque étape est invisible de l'extérieur.
///     L'appelant doit inspecter le contexte mutable pour voir les résultats.
/// </summary>
public class OrderOrchestrator(IEnumerable<IOrderProcessingStep> steps)
{
    public OrderProcessingContext Process(OrderDto order)
    {
        var context = new OrderProcessingContext { Order = order };
        foreach (var step in steps)
        {
            step.Execute(context);

            if (!context.IsValid && context.Errors.Count > 0)
            {
                break;
            }
        }

        return context;
    }
}
