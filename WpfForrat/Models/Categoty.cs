using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class Categoty
{
    public int IdCategory { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
