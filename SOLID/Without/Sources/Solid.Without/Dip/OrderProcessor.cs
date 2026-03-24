using Solid.Without.Models;

namespace Solid.Without.Dip;

// ANTI-PATTERN : Violation du principe d'inversion des dépendances (DIP)
// OrderProcessor dépend directement de classes concrètes (FileOrderRepository,
// SmtpEmailService). Le module de haut niveau est couplé aux détails d'implémentation.
// Conséquences :
//   - Impossible de tester unitairement sans écrire sur disque ou envoyer un e-mail
//   - Changer de mode de persistance ou de notification impose de modifier cette classe
//   - Aucune abstraction intermédiaire → couplage fort
public class OrderProcessor
{
    // ANTI-PATTERN : instanciation directe des dépendances concrètes
    // Le module de haut niveau CRÉE ses propres dépendances de bas niveau.
    private readonly FileOrderRepository _repository = new();
    private readonly SmtpEmailService _emailService = new();

    public string Process(Order order)
    {
        decimal total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        _repository.Save(order, total);
        _emailService.SendConfirmation(order.CustomerEmail, total);

        return $"Commande traitée : {total:C}";
    }
}

// ANTI-PATTERN : classe concrète bas niveau — aucune abstraction
// Le test de OrderProcessor est obligé de passer par FileOrderRepository,
// ce qui rend les tests lents, fragiles et dépendants du système de fichiers.
public class FileOrderRepository
{
    private readonly List<string> _records = [];

    public IReadOnlyList<string> Records => _records;

    public void Save(Order order, decimal total) => _records.Add($"[FICHIER] Commande de {order.CustomerEmail} : {total:C}");
}

// ANTI-PATTERN : classe concrète bas niveau — aucune abstraction
// Impossible de remplacer le service d'e-mail par un mock ou un stub dans les tests.
public class SmtpEmailService
{
    private readonly List<string> _sentEmails = [];

    public IReadOnlyList<string> SentEmails => _sentEmails;

    public void SendConfirmation(string email, decimal total) => _sentEmails.Add($"[SMTP] Confirmation envoyée à {email} pour {total:C}");
}
