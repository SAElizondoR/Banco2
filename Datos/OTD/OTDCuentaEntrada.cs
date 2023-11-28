namespace InterfazBanco.Datos.OTD;

public partial class OTDCuentaEntrada
{
    public int Id { get; set; }

    public int TipoCuenta { get; set; }

    public int? IdCliente { get; set; }

    public decimal Saldo { get; set; }
}
