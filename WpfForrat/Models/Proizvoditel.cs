using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class Proizvoditel
{
    public int IdProizvod { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
