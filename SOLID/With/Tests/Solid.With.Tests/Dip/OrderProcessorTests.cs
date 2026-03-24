using Solid.With.Dip;
using Solid.With.Models;

namespace Solid.With.Tests.Dip;

/// <summary>
/// Tests du OrderProcessor (DIP respecté).
/// Grâce à l'injection de dépendances, on peut injecter des implémentations
/// en mémoire et vérifier chaque interaction de manière isolée.
/// </summary>
public class OrderProcessorTests
{
    private static Order CreateValidOrder() =>
        new("client@example.com",
            [new OrderItem("Clavier", 50m, 2), new OrderItem("Souris", 25m, 1)]);

    // PRINCIPE DIP : On injecte des implémentations en mémoire pour les tests.
    // Aucun couplage avec le système de fichiers ou un serveur SMTP.
    [Fact]
    public void WhenProcessOrderThenReturnsFormattedResult()
    {
        var repository = new InMemoryOrderRepository();
        var notification = new InMemoryNotificationService();
        var processor = new OrderProcessor(repository, notification);

        string result = processor.Process(CreateValidOrder());

        Assert.Contains("125", result);
    }

    [Fact]
    public void WhenProcessOrderThenOrderIsSavedInRepository()
    {
        var repository = new InMemoryOrderRepository();
        var notification = new InMemoryNotificationService();
        var processor = new OrderProcessor(repository, notification);

        processor.Process(CreateValidOrder());

        Assert.Single(repository.Records);
        Assert.Contains("client@example.com", repository.Records[0]);
    }

    [Fact]
    public void WhenProcessOrderThenNotificationIsSent()
    {
        var repository = new InMemoryOrderRepository();
        var notification = new InMemoryNotificationService();
        var processor = new OrderProcessor(repository, notification);

        processor.Process(CreateValidOrder());

        Assert.Single(notification.SentMessages);
        Assert.Contains("client@example.com", notification.SentMessages[0]);
    }

    [Fact]
    public void WhenOrderIsNullThenThrowsArgumentNull()
    {
        var repository = new InMemoryOrderRepository();
        var notification = new InMemoryNotificationService();
        var processor = new OrderProcessor(repository, notification);

        Assert.Throws<ArgumentNullException>(() => processor.Process(null!));
    }

    // PRINCIPE DIP : On peut facilement créer un stub personnalisé pour
    // tester des scénarios spécifiques (par exemple, un repository qui échoue).
    [Fact]
    public void WhenRepositoryFailsThenExceptionPropagates()
    {
        var failingRepo = new FailingOrderRepository();
        var notification = new InMemoryNotificationService();
        var processor = new OrderProcessor(failingRepo, notification);

        Assert.Throws<InvalidOperationException>(() => processor.Process(CreateValidOrder()));
    }

    /// <summary>
    /// Stub qui simule une erreur de persistance — possible uniquement grâce à DIP.
    /// </summary>
    private sealed class FailingOrderRepository : IOrderRepository
    {
        public void Save(Order order, decimal total) =>
            throw new InvalidOperationException("Erreur simulée de persistance.");
    }
}
