namespace Solid.With.Ocp;

// PRINCIPE OCP : Ouvert à l'extension, fermé à la modification.
// Pour ajouter un nouveau type de remise, il suffit de créer une nouvelle
// implémentation de IDiscountStrategy — sans toucher au code existant.

/// <summary>
/// Abstraction d'une stratégie de remise.
/// </summary>
public interface IDiscountStrategy
{
    /// <summary>
    /// Applique la remise au montant donné.
    /// </summary>
    decimal Apply(decimal amount);
}

/// <summary>
/// Remise en pourcentage (ex : 10 %, 20 %).
/// </summary>
public class PercentageDiscount(decimal percentage) : IDiscountStrategy
{
    public decimal Apply(decimal amount) => amount * (1 - percentage / 100m);
}

/// <summary>
/// Remise à montant fixe (ex : -5 €, -10 €).
/// </summary>
public class FixedAmountDiscount(decimal discountAmount) : IDiscountStrategy
{
    public decimal Apply(decimal amount) => Math.Max(0, amount - discountAmount);
}

/// <summary>
/// Aucune remise appliquée.
/// </summary>
public class NoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal amount) => amount;
}

// PRINCIPE OCP en action : le DiscountCalculator accepte N'IMPORTE QUELLE
// stratégie de remise via l'abstraction. Ajouter "BuyTwoGetOneFree" se fait
// en créant une nouvelle classe, sans modifier DiscountCalculator.

/// <summary>
/// Calcule le montant après application d'une stratégie de remise.
/// </summary>
public class DiscountCalculator
{
    /// <summary>
    /// Applique la stratégie de remise au montant donné.
    /// </summary>
    public static decimal ApplyDiscount(decimal amount, IDiscountStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        return strategy.Apply(amount);
    }
}
