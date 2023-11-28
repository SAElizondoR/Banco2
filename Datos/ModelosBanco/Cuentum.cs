using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class Cuenta
{
    public int Id { get; set; }

    public int TipoCuenta { get; set; }

    public int? IdCliente { get; set; }

    public decimal Saldo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual TipoCuenta TipoCuentaNavigation { get; set; } = null!;

    public virtual ICollection<TransaccionBancarium> TransaccionBancaria { get; } = new List<TransaccionBancarium>();
}
