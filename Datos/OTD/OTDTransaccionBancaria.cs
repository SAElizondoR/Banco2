namespace InterfazBanco.Datos.ModelosBanco;

public partial class OTDTransaccionBancaria
{

    public int IdCuenta { get; set; }

    public int TipoTransaccion { get; set; }

    public decimal Importe { get; set; }

    public int? CuentaExterna { get; set; }
}
