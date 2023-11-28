using InterfazBanco.Datos.ModelosBanco;
using InterfazBanco.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterfazBanco.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ServicioCliente _servicio;

    private readonly ILogger<ClienteController> _logger;

    public ClienteController(ServicioCliente servicio,
        ILogger<ClienteController> logger)
    {
        _servicio = servicio;
        _logger = logger;
    }

    [HttpGet("obtenertodo")]
    public async Task<IEnumerable<Cliente>> Get()
    {
        return await _servicio.ObtenerTodos();
    }

    [HttpGet("obtenertodo2")]
    public async Task<IEnumerable<Cliente>> Get2()
    {
        return await _servicio.ObtenerTodos();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> ObtenerConId(int id)
    {
        var cliente = await _servicio.ObtenerPorId(id);

        if (cliente is null)
            return ClienteNoEncontrado(id);
        
        return cliente;
    }

    [Authorize(Policy = "Superadmin")]
    [HttpPost("agregar")]
    public async Task<IActionResult> Crear(Cliente cliente)
    {
        var clienteNuevo = await _servicio.Crear(cliente);

        return CreatedAtAction(nameof(ObtenerConId),
            new {id = clienteNuevo.Id}, clienteNuevo);
    }

    [Authorize(Policy = "Superadmin")]
    [HttpPut("actualizar/{id}")]
    public async Task<IActionResult> Actualizar(int id, Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest(new { message = $"El id. ({id}) del enlace " +
                $"no coincide con el id. ({id}) del " +
                "cuerpo de la solicitud." });
        
        var clienteAActualizar = await _servicio.ObtenerPorId(id);
        if (clienteAActualizar is not null)
        {
            await _servicio.Actualizar(id, cliente);
            return NoContent();
        }
        else
        {
            return ClienteNoEncontrado(id);
        }
    }

    [Authorize(Policy = "Superadmin")]
    [HttpDelete("borrar/{id}")]
    public async Task<IActionResult> Borrar(int id)
    {
        var clienteABorrar = await _servicio.ObtenerPorId(id);
        if (clienteABorrar is not null)
        {
            await _servicio.Borrar(id);
            return Ok();
        }
        else
        {
            return ClienteNoEncontrado(id);
        }
    }

    private NotFoundObjectResult ClienteNoEncontrado(int id)
    {
        return NotFound(
            new { message = $"El cliente con id. = {id} no existe." });
    }
}
