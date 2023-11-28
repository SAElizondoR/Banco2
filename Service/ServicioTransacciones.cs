using InterfazBanco.Datos;
using InterfazBanco.Datos.ModelosBanco;

namespace InterfazBanco.Servicios;

public class ServicioTransacciones
{
    private readonly BancoContext _contexto;

    public ServicioTransacciones(BancoContext context)
    {
        _contexto = context;
    }

    public async Task<TransaccionBancarium> Crear(OTDTransaccionBancaria otd)
    {
        var transaccion = new TransaccionBancarium
        {
            IdCuenta = otd.IdCuenta,
            TipoTransaccion = otd.TipoTransaccion,
            Importe = otd.Importe,
            CuentaExterna = otd.CuentaExterna
        };
        _contexto.TransaccionBancaria.Add(transaccion);
        await _contexto.SaveChangesAsync();
        return transaccion;
    }
}
