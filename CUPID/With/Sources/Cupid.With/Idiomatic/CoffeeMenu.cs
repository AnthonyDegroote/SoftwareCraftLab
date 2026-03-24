using Cupid.With.Models;

namespace Cupid.With.Idiomatic;

// CUPID Idiomatic ✓ — Le code est naturel pour un développeur C#.
//     Records immuables, LINQ, pattern matching, API fluide.
//     Pas de Builder verbeux Java-style, pas de getters/setters manuels.
//     Un dev C# lit ce code et se dit : "c'est comme ça que j'aurais fait."

/// <summary>
/// Carte des cafés du coffee shop.
/// Idiomatic : API simple, naturelle, qui utilise les features C# modernes.
/// </summary>
public static class CoffeeMenu
{
    // CUPID Idiomatic : propriétés statiques en lecture seule, expressives.
    public static Coffee Espresso(CoffeeSize size = CoffeeSize.Small) => size switch
    {
        CoffeeSize.Small => new("Espresso", size, new Money(2.50m)),
        CoffeeSize.Medium => new("Espresso", size, new Money(3.00m)),
        CoffeeSize.Large => new("Espresso", size, new Money(3.50m)),
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static Coffee Latte(CoffeeSize size = CoffeeSize.Medium) => size switch
    {
        CoffeeSize.Small => new("Latte", size, new Money(3.50m)),
        CoffeeSize.Medium => new("Latte", size, new Money(4.00m)),
        CoffeeSize.Large => new("Latte", size, new Money(4.50m)),
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static Coffee Cappuccino(CoffeeSize size = CoffeeSize.Medium) => size switch
    {
        CoffeeSize.Small => new("Cappuccino", size, new Money(3.00m)),
        CoffeeSize.Medium => new("Cappuccino", size, new Money(3.50m)),
        CoffeeSize.Large => new("Cappuccino", size, new Money(4.00m)),
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    // CUPID Idiomatic : helper method qui simplifie la création de lignes de commande.
    // Natural C# : CoffeeMenu.OrderLine(CoffeeMenu.Latte(), 2)

    /// <summary>
    /// Crée une ligne de commande à partir d'un café et d'une quantité.
    /// </summary>
    public static OrderLine OrderLine(Coffee coffee, int quantity = 1) => new(coffee, quantity);
}
