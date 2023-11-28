using InterfazBanco.Datos;
using InterfazBanco.Datos.ModelosBanco;

namespace InterfazBanco.Servicios;

public class ServicioTipoCuenta
{
    private readonly BancoContext _contexto;

    public ServicioTipoCuenta(BancoContext context)
    {
        _contexto = context;
    }

    public async Task<TipoCuenta?> ObtenerPorId(int id)
    {
        return await _contexto.TipoCuenta.FindAsync(id);
    }

}
