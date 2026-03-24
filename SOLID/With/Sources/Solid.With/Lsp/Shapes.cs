namespace Solid.With.Lsp;

// PRINCIPE LSP : Substitution de Liskov
// Tous les sous-types de IShape respectent le même contrat : CalculateArea()
// renvoie la surface correcte quelle que soit la forme. Aucune propriété mutable
// partagée ne crée d'effets de bord inattendus.

/// <summary>
/// Abstraction commune pour toutes les formes géométriques.
/// Chaque implémentation garantit un calcul d'aire cohérent avec sa géométrie.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Calcule l'aire de la forme.
    /// </summary>
    double CalculateArea();
}

/// <summary>
/// Rectangle défini par sa largeur et sa hauteur (immuable).
/// </summary>
public record Rectangle(double Width, double Height) : IShape
{
    // PRINCIPE LSP : Width et Height sont indépendants, pas d'effet de bord.
    public double CalculateArea() => Width * Height;
}

/// <summary>
/// Carré défini par la longueur de son côté (immuable).
/// </summary>
public record Square(double Side) : IShape
{
    // PRINCIPE LSP : Le carré n'hérite PAS de Rectangle.
    // Il a sa propre représentation (un seul côté), pas de conflit de contrat.
    public double CalculateArea() => Side * Side;
}

/// <summary>
/// Cercle défini par son rayon (immuable).
/// </summary>
public record Circle(double Radius) : IShape
{
    // PRINCIPE LSP : Facile d'ajouter de nouvelles formes sans casser le contrat.
    public double CalculateArea() => Math.PI * Radius * Radius;
}
