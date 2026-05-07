using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class PunktVidachi
{
    public int IdPunkta { get; set; }

    public int Number { get; set; }

    public string Adres { get; set; } = null!;

    public virtual ICollection<Zakaz> Zakazs { get; set; } = new List<Zakaz>();
}
