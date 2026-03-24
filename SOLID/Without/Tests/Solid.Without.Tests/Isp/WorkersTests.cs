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
        var worker = new HumanWorker();

        Assert.Equal("L'humain travaille.", worker.Work());
    }

    [Fact]
    public void WhenHumanWorkerEatsThenReturnsMessage()
    {
        var worker = new HumanWorker();

        Assert.Equal("L'humain mange.", worker.Eat());
    }

    [Fact]
    public void WhenRobotWorkerWorksThenReturnsMessage()
    {
        var worker = new RobotWorker();

        Assert.Equal("Le robot travaille.", worker.Work());
    }

    // ANTI-PATTERN : L'interface trop large force le robot à implémenter Eat()
    // et Sleep(), ce qui ne peut que lever une exception → code fragile.
    [Fact]
    public void WhenRobotWorkerEatsThenThrowsNotSupported()
    {
        var worker = new RobotWorker();

        Assert.Throws<NotSupportedException>(() => worker.Eat());
    }

    [Fact]
    public void WhenRobotWorkerSleepsThenThrowsNotSupported()
    {
        var worker = new RobotWorker();

        Assert.Throws<NotSupportedException>(() => worker.Sleep());
    }

    [Fact]
    public void WhenRobotWorkerAttendsMeetingThenThrowsNotSupported()
    {
        var worker = new RobotWorker();

        Assert.Throws<NotSupportedException>(() => worker.AttendMeeting());
    }
}
