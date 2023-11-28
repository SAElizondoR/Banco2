using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class TransaccionBancarium
{
    public int Id { get; set; }

    public int IdCuenta { get; set; }

    public int TipoTransaccion { get; set; }

    public decimal Importe { get; set; }

    public int? CuentaExterna { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Cuenta IdCuentaNavigation { get; set; } = null!;

    public virtual TipoTransaccion TipoTransaccionNavigation { get; set; } = null!;
}
