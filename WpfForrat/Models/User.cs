using System;
using System.Collections.Generic;

namespace WpfForrat.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdRole { get; set; }

    public string Fio { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Zakaz> Zakazs { get; set; } = new List<Zakaz>();
}
