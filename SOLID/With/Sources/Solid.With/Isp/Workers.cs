namespace Solid.With.Isp;

// PRINCIPE ISP : Ségrégation des interfaces
// Chaque interface est petite et ciblée. Une classe n'implémente QUE
// les interfaces pertinentes pour elle. Aucune méthode inutile imposée.

/// <summary>
/// Capacité de travailler.
/// </summary>
public interface IWorkable
{
    string Work();
}

/// <summary>
/// Capacité de se nourrir (propre aux êtres vivants).
/// </summary>
public interface IFeedable
{
    string Eat();
}

/// <summary>
/// Capacité de dormir (propre aux êtres vivants).
/// </summary>
public interface ISleepable
{
    string Sleep();
}

/// <summary>
/// Capacité d'assister à une réunion.
/// </summary>
public interface IMeetingAttendee
{
    string AttendMeeting();
}

// PRINCIPE ISP : HumanWorker implémente TOUTES les interfaces car elles
// sont toutes pertinentes pour un humain.

/// <summary>
/// Travailleur humain — capable de travailler, manger, dormir et assister à des réunions.
/// </summary>
public class HumanWorker : IWorkable, IFeedable, ISleepable, IMeetingAttendee
{
    public string Work() => "L'humain travaille.";
    public string Eat() => "L'humain mange.";
    public string Sleep() => "L'humain dort.";
    public string AttendMeeting() => "L'humain assiste à la réunion.";
}

// PRINCIPE ISP : RobotWorker n'implémente QUE IWorkable.
// Il n'est pas forcé d'implémenter Eat(), Sleep() ou AttendMeeting().

/// <summary>
/// Travailleur robotique — capable uniquement de travailler.
/// </summary>
public class RobotWorker : IWorkable
{
    public string Work() => "Le robot travaille.";
}
