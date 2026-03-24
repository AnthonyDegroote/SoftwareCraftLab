using Solid.With.Lsp;

namespace Solid.With.Tests.Lsp;

/// <summary>
/// Tests des formes géométriques (LSP respecté).
/// Toutes les implémentations de IShape sont substituables :
/// CalculateArea() renvoie toujours le résultat correct.
/// </summary>
public class ShapesTests
{
    // PRINCIPE LSP : chaque forme est substituable — [Theory] vérifie le contrat IShape
    // pour toutes les implémentations en une seule méthode de test.
    public static TheoryData<IShape, double> ShapeAreaCases => new()
    {
        { new Rectangle(5, 4), 20 },
        { new Square(5), 25 },
        { new Circle(3), Math.PI * 9 }
    };

    [Theory]
    [MemberData(nameof(ShapeAreaCases))]
    public void WhenCalculateAreaThenReturnsCorrectValue(IShape shape, double expectedArea) => Assert.Equal(expectedArea, shape.CalculateArea(), precision: 10);

    // PRINCIPE LSP : on peut traiter une collection de IShape de manière uniforme.
    // Chaque forme respecte le contrat — pas de surprise, pas d'exception.
    [Fact]
    public void WhenMixedShapesThenAllAreasAreCorrect()
    {
        // Arrange
        IShape[] shapes =
        [
            new Rectangle(10, 5),
            new Square(4),
            new Circle(1)
        ];

        // Act
        double totalArea = shapes.Sum(s => s.CalculateArea());

        // Assert
        // 50 + 16 + π ≈ 69.14
        Assert.Equal(50 + 16 + Math.PI, totalArea, precision: 10);
    }
}
