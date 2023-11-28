namespace InterfazBanco.Datos.OTD;

public partial class OTDCuentaSalida
{
    public int Id { get; set; }

    public string NombreCuenta { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public decimal Saldo { get; set; }
    public DateTime FechaRegistro { get; set; }
}
