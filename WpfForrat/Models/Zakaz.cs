using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class Zakaz
{
    public int IdZakaz { get; set; }

    public int IdUser { get; set; }

    public int Kod { get; set; }

    public int IdStatus { get; set; }

    public int IdAdres { get; set; }

    public DateOnly DateZakaz { get; set; }

    public DateOnly DateDostav { get; set; }

    public virtual PunktVidachi IdAdresNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<SostavZakaz> SostavZakazs { get; set; } = new List<SostavZakaz>();
}
