using Solid.With.Lsp;

namespace Solid.With.Tests.Lsp;

/// <summary>
/// Tests des formes géométriques (LSP respecté).
/// Toutes les implémentations de IShape sont substituables :
/// CalculateArea() renvoie toujours le résultat correct.
/// </summary>
public class ShapesTests
{
    [Fact]
    public void WhenRectangleThenAreaIsWidthTimesHeight()
    {
        IShape shape = new Rectangle(5, 4);

        Assert.Equal(20, shape.CalculateArea());
    }

    [Fact]
    public void WhenSquareThenAreaIsSideSquared()
    {
        IShape shape = new Square(5);

        Assert.Equal(25, shape.CalculateArea());
    }

    [Fact]
    public void WhenCircleThenAreaIsPiTimesRadiusSquared()
    {
        IShape shape = new Circle(3);

        Assert.Equal(Math.PI * 9, shape.CalculateArea(), precision: 10);
    }

    // PRINCIPE LSP : on peut traiter une collection de IShape de manière uniforme.
    // Chaque forme respecte le contrat — pas de surprise, pas d'exception.
    [Fact]
    public void WhenMixedShapesThenAllAreasAreCorrect()
    {
        IShape[] shapes =
        [
            new Rectangle(10, 5),
            new Square(4),
            new Circle(1)
        ];

        double totalArea = shapes.Sum(s => s.CalculateArea());

        // 50 + 16 + π ≈ 69.14
        Assert.Equal(50 + 16 + Math.PI, totalArea, precision: 10);
    }
}
