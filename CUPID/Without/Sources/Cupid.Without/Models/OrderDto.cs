namespace Cupid.Without.Models;

// SOLID ✓ — SRP respecté : le modèle ne contient que des données.
// PAS CUPID Idiomatic ✗ — Accesseurs Java-style (GetX/SetX) au lieu de propriétés C#.
// PAS CUPID Domain ✗ — Nommage technique ("Dto", "BeverageItem") sans langage métier.
//     Un barista parle de "café", "latte", "espresso" — pas de "BeverageItemDto".

/// <summary>
/// Représente un article de boisson dans une commande.
/// </summary>
public class BeverageItemDto
{
    private string _name = "";
    private decimal _unitPrice;
    private int _quantity;
    private string _size = "";

    // ANTI-CUPID Idiomatic : en C#, on utilise des propriétés, pas des getters/setters Java.
    public string GetName() => _name;
    public void SetName(string value) => _name = value;

    public decimal GetUnitPrice() => _unitPrice;
    public void SetUnitPrice(decimal value) => _unitPrice = value;

    public int GetQuantity() => _quantity;
    public void SetQuantity(int value) => _quantity = value;

    public string GetSize() => _size;
    public void SetSize(string value) => _size = value;
}

// ANTI-CUPID Domain : "OrderDto" est un terme technique.
// Le métier dirait "Commande de café", pas "Data Transfer Object".
// Le modèle est anémique : aucun comportement, que des données brutes.

/// <summary>
/// Représente une commande client (modèle anémique).
/// </summary>
public class OrderDto
{
    private readonly List<BeverageItemDto> _items = [];
    private string _customerEmail = "";
    private string _customerName = "";

    public string GetCustomerEmail() => _customerEmail;
    public void SetCustomerEmail(string value) => _customerEmail = value;

    public string GetCustomerName() => _customerName;
    public void SetCustomerName(string value) => _customerName = value;

    public List<BeverageItemDto> GetItems() => _items;

    public void AddItem(BeverageItemDto item) => _items.Add(item);
}
