namespace Solid.Without.Ocp;

// ANTI-PATTERN : Violation du principe ouvert/fermé (OCP)
// Pour ajouter un nouveau type de remise, il faut MODIFIER cette classe
// au lieu de l'étendre. Chaque ajout de remise nécessite de toucher au switch,
// ce qui augmente le risque de régression et viole le principe « fermé à la modification ».
public class DiscountCalculator
{
    /// <summary>
    /// Calcule le montant après remise.
    /// Ajouter un nouveau type de remise impose de modifier ce code existant.
    /// </summary>
    public static decimal ApplyDiscount(decimal amount, string discountType, decimal discountValue)
    {
        // ANTI-PATTERN : switch fermé — chaque nouvelle remise = modification du code source
        return discountType switch
        {
            "Percentage" => amount * (1 - discountValue / 100m),
            "FixedAmount" => Math.Max(0, amount - discountValue),
            "BuyTwoGetOneFree" => amount * 2m / 3m,
            // Pour ajouter "LoyaltyPoints", "SeasonalSale", etc.,
            // il faut ajouter un cas ici → violation OCP
            _ => amount
        };
    }
}
