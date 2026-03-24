namespace Solid.Without.Isp;

// ANTI-PATTERN : Violation du principe de ségrégation des interfaces (ISP)
// Une seule interface « fourre-tout » force les implémentations à fournir
// des méthodes qui ne les concernent pas.
// Conséquence : les classes sont obligées de lever des exceptions ou de
// laisser des corps vides pour les méthodes non pertinentes.
public interface IWorker
{
    string Work();
    string Eat();
    string Sleep();
    string AttendMeeting();
}

/// <summary>
/// Un travailleur humain peut raisonnablement implémenter toutes les méthodes.
/// </summary>
public class HumanWorker : IWorker
{
    public string Work() => "L'humain travaille.";
    public string Eat() => "L'humain mange.";
    public string Sleep() => "L'humain dort.";
    public string AttendMeeting() => "L'humain assiste à la réunion.";
}

// ANTI-PATTERN : RobotWorker est forcé d'implémenter Eat() et Sleep()
// alors que ces actions n'ont aucun sens pour un robot.
// L'interface trop large impose des méthodes inutiles → violation ISP.
public class RobotWorker : IWorker
{
    public string Work() => "Le robot travaille.";

    // ANTI-PATTERN : méthode imposée par l'interface mais non applicable
    public string Eat() =>
        throw new NotSupportedException("Un robot ne mange pas.");

    // ANTI-PATTERN : méthode imposée par l'interface mais non applicable
    public string Sleep() =>
        throw new NotSupportedException("Un robot ne dort pas.");

    // ANTI-PATTERN : méthode imposée par l'interface mais non applicable
    public string AttendMeeting() =>
        throw new NotSupportedException("Un robot n'assiste pas aux réunions.");
}
