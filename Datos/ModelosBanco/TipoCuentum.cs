using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class TipoCuenta
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Cuenta> Cuenta { get; } = new List<Cuenta>();
}
