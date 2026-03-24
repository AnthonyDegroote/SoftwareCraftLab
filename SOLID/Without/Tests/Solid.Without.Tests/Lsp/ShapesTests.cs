using Solid.Without.Lsp;

namespace Solid.Without.Tests.Lsp;

/// <summary>
/// Tests démontrant la violation de LSP avec Rectangle/Square.
/// Le test clé montre que substituer un Square à un Rectangle
/// produit un résultat inattendu.
/// </summary>
public class ShapesTests
{
    [Fact]
    public void WhenRectangleThenAreaIsWidthTimesHeight()
    {
        // Arrange
        var rectangle = new Rectangle { Width = 5, Height = 4 };

        // Act & Assert
        Assert.Equal(20, rectangle.CalculateArea());
    }

    [Fact]
    public void WhenSquareThenAreaIsSideTimeSide()
    {
        // Arrange
        var square = new Square { Width = 5 };

        // Act & Assert
        // Width = 5 a aussi mis Height = 5
        Assert.Equal(25, square.CalculateArea());
    }

    // ANTI-PATTERN : Ce test démontre la VIOLATION de LSP.
    // En substituant un Square à un Rectangle, le comportement change
    // de façon inattendue : modifier Height écrase Width.
    [Fact]
    public void WhenSquareUsedAsRectangleThenAreaIsUnexpected()
    {
        // Arrange
        Rectangle shape = new Square();
        shape.Width = 5;
        shape.Height = 4;

        // Act
        // On s'attend à 20 (5×4), mais on obtient 16 (4×4)
        // car Height = 4 a aussi modifié Width → violation LSP
        double area = shape.CalculateArea();

        // Assert
        Assert.NotEqual(20, area);   // Le contrat de Rectangle est brisé
        Assert.Equal(16, area);       // Car le carré a forcé Width = Height = 4
    }
}
