using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class EdIzm
{
    public int IdEdIzm { get; set; }

    public string EdIzm1 { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
