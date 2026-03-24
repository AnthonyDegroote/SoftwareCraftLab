using Solid.Without.Isp;

namespace Solid.Without.Tests.Isp;

/// <summary>
/// Tests démontrant la violation d'ISP.
/// RobotWorker est obligé de lever NotSupportedException sur les méthodes
/// qu'il ne peut pas implémenter.
/// </summary>
public class WorkersTests
{
    [Fact]
    public void WhenHumanWorkerWorksThenReturnsMessage()
    {
        // Arrange
        var worker = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain travaille.", worker.Work());
    }

    [Fact]
    public void WhenHumanWorkerEatsThenReturnsMessage()
    {
        // Arrange
        var worker = new HumanWorker();

        // Act & Assert
        Assert.Equal("L'humain mange.", worker.Eat());
    }

    [Fact]
    public void WhenRobotWorkerWorksThenReturnsMessage()
    {
        // Arrange
        var worker = new RobotWorker();

        // Act & Assert
        Assert.Equal("Le robot travaille.", worker.Work());
    }

    // ANTI-PATTERN : L'interface trop large force le robot à implémenter Eat()
    // et Sleep(), ce qui ne peut que lever une exception → code fragile.
    [Fact]
    public void WhenRobotWorkerEatsThenThrowsNotSupported()
    {
        // Arrange
        var worker = new RobotWorker();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => worker.Eat());
    }

    [Fact]
    public void WhenRobotWorkerSleepsThenThrowsNotSupported()
    {
        // Arrange
        var worker = new RobotWorker();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => worker.Sleep());
    }

    [Fact]
    public void WhenRobotWorkerAttendsMeetingThenThrowsNotSupported()
    {
        // Arrange
        var worker = new RobotWorker();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => worker.AttendMeeting());
    }
}
