namespace Solid.With.Srp;

// PRINCIPE SRP : NotificationService ne s'occupe QUE de l'envoi de notifications.
// On peut passer d'un e-mail à un SMS sans impacter le reste du traitement.

/// <summary>
/// Envoie des notifications de confirmation (implémentation en mémoire pour la démonstration).
/// </summary>
public class NotificationService
{
    private readonly List<string> _sentMessages = [];

    public IReadOnlyList<string> SentMessages => _sentMessages;

    /// <summary>
    /// Envoie une confirmation au client.
    /// </summary>
    public void SendConfirmation(string email, decimal total)
    {
        ArgumentNullException.ThrowIfNull(email);
        _sentMessages.Add($"Confirmation envoyée à {email} pour un total de {total:C}");
    }
}
