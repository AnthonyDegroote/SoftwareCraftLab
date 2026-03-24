using Solid.Without.Dip;
using Solid.Without.Models;

namespace Solid.Without.Tests.Dip;

/// <summary>
/// Tests du OrderProcessor (violation DIP).
/// Le processeur crée lui-même ses dépendances concrètes.
/// On ne peut pas injecter de stubs/mocks → tests couplés aux implémentations réelles.
/// </summary>
public class OrderProcessorTests
{
    // ANTI-PATTERN : On ne peut pas substituer le repository ni le service d'e-mail.
    // Le test est obligé d'utiliser les vraies implémentations (FileOrderRepository, SmtpEmailService).
    private readonly OrderProcessor _processor = new();

    private static Order CreateValidOrder() =>
        new("client@example.com",
            [new OrderItem("Clavier", 50m, 2), new OrderItem("Souris", 25m, 1)]);

    [Fact]
    public void WhenProcessOrderThenReturnsFormattedResult()
    {
        string result = _processor.Process(CreateValidOrder());

        Assert.Contains("125", result);
    }

    // ANTI-PATTERN : Impossible de vérifier les appels au repository ou au service
    // d'e-mail sans accéder aux champs internes des classes concrètes.
    // On ne teste que le résultat final, pas le comportement intermédiaire.
    [Fact]
    public void WhenProcessOrderThenResultContainsConfirmation()
    {
        string result = _processor.Process(CreateValidOrder());

        Assert.Contains("Commande traitée", result);
    }
}
