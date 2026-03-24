namespace Cupid.Without.Unix;

// SOLID ✓ — SRP (ne fait que de la validation), OCP (règles extensibles via injection).
// PAS CUPID Unix ✗ — Sur-ingénierie : un framework générique de validation au lieu d'un
//     simple validateur de commandes. "Fait une chose" mais en mode usine à gaz.
//     Un développeur doit comprendre le système de règles, les options, et le callback
//     pour valider une simple commande. Le principe Unix dit : "fais UNE chose, BIEN."
//     Ici, on fait "n'importe quelle validation, de n'importe quoi, de n'importe quelle façon".

/// <summary>
/// Résultat d'un moteur de validation générique.
/// </summary>
public record ValidationEngineResult(bool IsValid, IReadOnlyList<string> Errors);

/// <summary>
/// Options de configuration du moteur de validation.
/// </summary>
public class ValidationOptions
{
    public bool StopOnFirstError { get; set; } = true;
    public Action<string>? OnErrorCallback { get; set; }
}

/// <summary>
/// Moteur de validation générique configurable.
/// SOLID ✓ — ouvert à l'extension (ajout de règles sans modifier la classe).
/// ANTI-CUPID Unix — trop générique, trop configurable, dur à comprendre d'un coup d'œil.
/// </summary>
public class ConfigurableValidationEngine<T>
{
    private readonly List<(Func<T, bool> Predicate, string ErrorMessage)> _rules = [];
    private readonly ValidationOptions _options = new();

    /// <summary>
    /// Ajoute une règle de validation avec son message d'erreur.
    /// </summary>
    public ConfigurableValidationEngine<T> AddRule(Func<T, bool> predicate, string errorMessage)
    {
        _rules.Add((predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Configure l'arrêt à la première erreur.
    /// </summary>
    public ConfigurableValidationEngine<T> WithStopOnFirstError(bool stop)
    {
        _options.StopOnFirstError = stop;
        return this;
    }

    /// <summary>
    /// Configure un callback d'erreur.
    /// </summary>
    public ConfigurableValidationEngine<T> WithErrorCallback(Action<string> callback)
    {
        _options.OnErrorCallback = callback;
        return this;
    }

    /// <summary>
    /// Exécute toutes les règles sur l'entité.
    /// Un développeur doit configurer le moteur avant de l'utiliser → friction.
    /// </summary>
    public ValidationEngineResult Validate(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        List<string> errors = [];

        foreach (var (predicate, errorMessage) in _rules)
        {
            if (!predicate(entity))
            {
                errors.Add(errorMessage);
                _options.OnErrorCallback?.Invoke(errorMessage);

                if (_options.StopOnFirstError)
                {
                    break;
                }
            }
        }

        return new ValidationEngineResult(errors.Count == 0, errors);
    }
}
