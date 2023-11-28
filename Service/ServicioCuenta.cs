using InterfazBanco.Datos;
using InterfazBanco.Datos.ModelosBanco;
using InterfazBanco.Datos.OTD;
using Microsoft.EntityFrameworkCore;

namespace InterfazBanco.Servicios;

public class AccountService
{
    private readonly BancoContext _contexto;

    public AccountService(BancoContext context)
    {
        _contexto = context;
    }

    public async Task<IEnumerable<OTDCuentaSalida>> ObtenerTodos()
    {
        return await _contexto.Cuenta.Select(c => new OTDCuentaSalida
        {
            Id = c.Id,
            NombreCuenta = c.TipoCuentaNavigation.Nombre,
            NombreCliente = c.IdClienteNavigation != null ?
                c.IdClienteNavigation.Nombre : "",
            Saldo = c.Saldo,
            FechaRegistro = c.FechaRegistro
        }).ToListAsync();
    }

    public async Task<OTDCuentaSalida?> ObtenerOTDPorId(int id)
    {
        return await _contexto.Cuenta
            .Where(c => c.Id == id)
            .Select(c => new OTDCuentaSalida
            {
                Id = c.Id,
                NombreCuenta = c.TipoCuentaNavigation.Nombre,
                NombreCliente = c.IdClienteNavigation != null ?
                    c.IdClienteNavigation.Nombre : "",
                Saldo = c.Saldo,
                FechaRegistro = c.FechaRegistro
            }).SingleOrDefaultAsync();
    }

    public async
        Task<IEnumerable<OTDCuentaSalida>> ObtenerOTDPorIdCliente(int id)
    {
        return await _contexto.Cuenta
            .Where(c => c.IdCliente == id)
            .Select(c => new OTDCuentaSalida
            {
                Id = c.Id,
                NombreCuenta = c.TipoCuentaNavigation.Nombre,
                NombreCliente = c.IdClienteNavigation != null ?
                    c.IdClienteNavigation.Nombre : "",
                Saldo = c.Saldo,
                FechaRegistro = c.FechaRegistro
            }).ToListAsync();
    }

    public async Task<Cuenta?> ObtenerPorId(int id)
    {
        return await _contexto.Cuenta.FindAsync(id);
    }

    public async Task<Cuenta> Crear(OTDCuentaEntrada OTDCuentaNuevo)
    {
        var cuentaNueva = new Cuenta
        {
            TipoCuenta = OTDCuentaNuevo.TipoCuenta,
            IdCliente = OTDCuentaNuevo.IdCliente,
            Saldo = OTDCuentaNuevo.Saldo
        };

        _contexto.Cuenta.Add(cuentaNueva);
        await _contexto.SaveChangesAsync();
        return cuentaNueva;
    }

    public async Task Actualizar(OTDCuentaEntrada cuenta)
    {
        var cuentaExistente = await ObtenerPorId(cuenta.Id);

        if (cuentaExistente is not null)
        {
            cuentaExistente.TipoCuenta = cuenta.TipoCuenta;
            cuentaExistente.IdCliente = cuenta.IdCliente;
            cuentaExistente.Saldo = cuenta.Saldo;
            await _contexto.SaveChangesAsync();
        }
    }

    public async Task Borrar(int id)
    {
        var cuentaAEliminar = await ObtenerPorId(id);

        if (cuentaAEliminar is not null)
        {
            _contexto.Cuenta.Remove(cuentaAEliminar);
            await _contexto.SaveChangesAsync();
        }
    }

    public async Task CambiarSaldo(int id, decimal cambio)
    {
        var cuenta = await ObtenerPorId(id);
        cuenta!.Saldo += cambio;
        await _contexto.SaveChangesAsync();
    }
}
