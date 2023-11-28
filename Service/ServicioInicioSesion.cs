using InterfazBanco.Datos;
using InterfazBanco.Datos.ModelosBanco;
using Microsoft.EntityFrameworkCore;

namespace InterfazBanco.Servicios;

public class ServicioInicioSesion
{
    private readonly BancoContext _contexto;

    public ServicioInicioSesion(BancoContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Administrador?> ObtenerAdmin(OTDAdmin admin)
    {
        return await _contexto.Administradors
            .SingleOrDefaultAsync(x => x.CorreoE == admin.CorreoE &&
            x.Contra == admin.Contra);
    }

    public async Task<Cliente?> ObtenerCliente(OTDAdmin cliente)
    {
        return await _contexto.Clientes.SingleOrDefaultAsync(x =>
            x.CorreoE == cliente.CorreoE && x.Pwd == cliente.Contra);
    }
}