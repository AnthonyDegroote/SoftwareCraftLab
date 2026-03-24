using Cupid.Without.Models;

namespace Cupid.Without.Idiomatic;

// SOLID ✓ — SRP (ne fait que la construction d'objets).
// PAS CUPID Idiomatic ✗ — Builder Java-style verbeux et mutable.
//     En C# moderne, un record avec `with` ou un constructeur primaire est idiomatique.
//     Ce builder est courant en Java, mais en C# il ajoute de la cérémonie inutile.
//     Un développeur C# s'attendrait à : new Order("Alice", "alice@mail.com", [...])

/// <summary>
/// Builder Java-style pour construire un OrderDto.
/// </summary>
public class OrderDtoBuilder
{
    // ANTI-CUPID Idiomatic : état mutable interne, chaînage verbeux
    private readonly OrderDto _order = new();

    public OrderDtoBuilder WithCustomerName(string name)
    {
        _order.SetCustomerName(name);
        return this;
    }

    public OrderDtoBuilder WithCustomerEmail(string email)
    {
        _order.SetCustomerEmail(email);
        return this;
    }

    public OrderDtoBuilder AddBeverage(string name, decimal price, int quantity, string size)
    {
        var item = new BeverageItemDto();
        item.SetName(name);
        item.SetUnitPrice(price);
        item.SetQuantity(quantity);
        item.SetSize(size);
        _order.AddItem(item);
        return this;
    }

    /// <summary>
    /// Construit l'objet final. Attention : le builder conserve une référence
    /// vers l'objet mutable — appeler Build() deux fois renvoie le MÊME objet.
    /// </summary>
    public OrderDto Build() => _order;
}
