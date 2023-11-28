using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NumeroTelefono { get; set; } = null!;

    public string? CorreoE { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Pwd { get; set; } = null!;

    public virtual ICollection<Cuenta> Cuenta { get; } = new List<Cuenta>();
}
