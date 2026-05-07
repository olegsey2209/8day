using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfForrat.Models;

public partial class Tovar
{
    public int IdTovar { get; set; }

    public string Articul { get; set; } = null!;

    public decimal Money { get; set; }

    public string Opisanie { get; set; } = null!;

    public int IdEdIzm { get; set; }

    public int IdPost { get; set; }

    public int IdProizvod { get; set; }

    public int IdCategory { get; set; }

    public int Sale { get; set; }

    public int KolSklad { get; set; }

    public string? Fhoto { get; set; }

    public string NameTovar { get; set; } = null!;

    public virtual Categoty IdCategoryNavigation { get; set; } = null!;

    public virtual EdIzm IdEdIzmNavigation { get; set; } = null!;

    public virtual Postavshik IdPostNavigation { get; set; } = null!;

    public virtual Proizvoditel IdProizvodNavigation { get; set; } = null!;

    public virtual ICollection<SostavZakaz> SostavZakazs { get; set; } = new List<SostavZakaz>();
    [NotMapped]
    public decimal PriceWithDiscount { get; set; }
}
