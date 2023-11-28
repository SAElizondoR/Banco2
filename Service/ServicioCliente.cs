using InterfazBanco.Datos;
using InterfazBanco.Datos.ModelosBanco;
using Microsoft.EntityFrameworkCore;

namespace InterfazBanco.Servicios;

public class ServicioCliente
{
    private readonly BancoContext _contexto = new();

    public ServicioCliente(BancoContext context)
    {
        _contexto = context;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodos()
    {
        return await _contexto.Clientes.ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorId(int id)
    {
        return await _contexto.Clientes.FindAsync(id);
    }

    public async Task<Cliente> Crear(Cliente clienteNuevo)
    {
        _contexto.Clientes.Add(clienteNuevo);
        await _contexto.SaveChangesAsync();
        return clienteNuevo;
    }

    public async Task Actualizar(int id, Cliente cliente)
    {
        var clienteExistente = await ObtenerPorId(id);

        if (clienteExistente is not null)
        {
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.NumeroTelefono = cliente.NumeroTelefono;
            clienteExistente.CorreoE = cliente.CorreoE;
            await _contexto.SaveChangesAsync();
        }
    }

    public async Task Borrar(int id)
    {
        var clienteAEliminar = await ObtenerPorId(id);

        if (clienteAEliminar is not null)
        {
            _contexto.Clientes.Remove(clienteAEliminar);
            await _contexto.SaveChangesAsync();
        }
    }
}
