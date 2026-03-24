using Solid.With.Isp;

namespace Solid.With.Tests.Isp;

/// <summary>
/// Tests des travailleurs (ISP respecté).
/// Chaque classe n'implémente que les interfaces pertinentes.
/// Aucune exception NotSupportedException — le compilateur empêche
/// d'appeler des méthodes non applicables.
/// </summary>
public class WorkersTests
{
    [Fact]
    public void WhenHumanWorkerWorksThenReturnsMessage()
    {
        // Arrange
        IWorkable worker = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain travaille.", worker.Work());
    }

    [Fact]
    public void WhenHumanWorkerEatsThenReturnsMessage()
    {
        // Arrange
        IFeedable human = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain mange.", human.Eat());
    }

    [Fact]
    public void WhenHumanWorkerSleepsThenReturnsMessage()
    {
        // Arrange
        ISleepable human = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain dort.", human.Sleep());
    }

    [Fact]
    public void WhenHumanWorkerAttendsMeetingThenReturnsMessage()
    {
        // Arrange
        IMeetingAttendee human = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain assiste à la réunion.", human.AttendMeeting());
    }

    [Fact]
    public void WhenRobotWorkerWorksThenReturnsMessage()
    {
        // Arrange
        IWorkable worker = new RobotWorker();

        // Act & Assert
        Assert.Equal("Le robot travaille.", worker.Work());
    }

    // PRINCIPE ISP : Pas de test pour Robot.Eat() ou Robot.Sleep()
    // car ces méthodes N'EXISTENT PAS sur RobotWorker.
    // Le compilateur protège contre les appels invalides — pas besoin d'exceptions à l'exécution.
    [Fact]
    public void WhenRobotWorkerThenOnlyImplementsIWorkable()
    {
        // Arrange
        var robot = new RobotWorker();

        // Assert
        Assert.IsAssignableFrom<IWorkable>(robot);
        Assert.IsNotAssignableFrom<IFeedable>(robot);     // Robot n'implémente PAS IFeedable
        Assert.IsNotAssignableFrom<ISleepable>(robot);     // Robot n'implémente PAS ISleepable
    }
}
