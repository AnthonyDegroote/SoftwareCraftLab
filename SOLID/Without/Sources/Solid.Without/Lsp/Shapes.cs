namespace Solid.Without.Lsp;

// ANTI-PATTERN : Violation du principe de substitution de Liskov (LSP)
// Rectangle expose des setters indépendants pour Width et Height.
// Square les redéfinit pour maintenir l'invariant carré, mais cela
// brise le contrat implicite de Rectangle : un appelant qui modifie
// Width ne s'attend PAS à ce que Height change aussi.
public class Rectangle
{
    public virtual double Width { get; set; }
    public virtual double Height { get; set; }

    public double CalculateArea() => Width * Height;
}

// ANTI-PATTERN : Square hérite de Rectangle mais modifie le comportement
// des propriétés de manière inattendue. Un code qui fonctionne avec
// un Rectangle échouera avec un Square :
//
//   Rectangle r = new Square();
//   r.Width = 5;
//   r.Height = 4;
//   r.CalculateArea() renvoie 16 (4×4) au lieu de 20 (5×4)
//
// Le sous-type n'est PAS substituable au type de base → violation LSP.
public class Square : Rectangle
{
    public override double Width
    {
        get => base.Width;
        set
        {
            // ANTI-PATTERN : effet de bord — modifier Width modifie aussi Height
            base.Width = value;
            base.Height = value;
        }
    }

    public override double Height
    {
        get => base.Height;
        set
        {
            // ANTI-PATTERN : effet de bord — modifier Height modifie aussi Width
            base.Width = value;
            base.Height = value;
        }
    }
}
