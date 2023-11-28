using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class TipoTransaccion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<TransaccionBancarium> TransaccionBancaria { get; } = new List<TransaccionBancarium>();
}
