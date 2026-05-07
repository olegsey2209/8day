using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class SostavZakaz
{
    public int IdTovar { get; set; }

    public int IdZakaz { get; set; }

    public int KolZakaz { get; set; }

    public int IdSostavZakaz { get; set; }

    public virtual Tovar IdTovarNavigation { get; set; } = null!;

    public virtual Zakaz IdZakazNavigation { get; set; } = null!;
}
