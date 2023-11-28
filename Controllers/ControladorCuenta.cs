using InterfazBanco.Datos.ModelosBanco;
using InterfazBanco.Datos.OTD;
using InterfazBanco.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterfazBanco.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _servicioCuenta;
    private readonly ServicioTipoCuenta _servicioTipoCuenta;
    private readonly ServicioCliente _servicioCliente;

    private readonly ILogger<ClienteController> _logger;

    public AccountController(AccountService servicioCuenta,
        ServicioTipoCuenta servicioTipoCuenta, ServicioCliente servicioCliente,
        ILogger<ClienteController> logger)
    {
        this._servicioCuenta = servicioCuenta;
        this._servicioTipoCuenta = servicioTipoCuenta;
        this._servicioCliente = servicioCliente;
        _logger = logger;
    }

    [HttpGet("obtenertodo")]
    public async Task<IEnumerable<OTDCuentaSalida>> GetAll()
    {
        return await _servicioCuenta.ObtenerTodos();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OTDCuentaSalida>> GetByID(int id)
    {
        var cuenta = await _servicioCuenta.ObtenerOTDPorId(id);

        if (cuenta is null)
            return CuentaNoEncontrada(id);
        
        return cuenta;
    }

    [Authorize(Policy = "Superadmin")]
    [HttpPost]
    public async Task<IActionResult> Crear(OTDCuentaEntrada cuenta)
    {
        string resultadoValidacion = await ValidarCuenta(cuenta);

        if (!resultadoValidacion.Equals("valida"))
            return BadRequest(new { message = resultadoValidacion });
        
        var cuentaNueva = await _servicioCuenta.Crear(cuenta);

        return CreatedAtAction(nameof(GetByID),
            new {id = cuentaNueva.Id}, cuentaNueva);
    }

    [Authorize(Policy = "Superadmin")]
    [HttpPut("actualizar/{id}")]
    public async Task<IActionResult> Actualizar(int id, OTDCuentaEntrada cuenta)
    {
        if (id != cuenta.Id)
            return BadRequest(new { message = $"El id. ({id}) del enlace " +
                $"no coincide con el id. ({id}) del " +
                "cuerpo de la solicitud." });
        
        var cuentaAActualizar = await _servicioCuenta.ObtenerPorId(id);
        if (cuentaAActualizar is not null)
        {
            string resultadoValidacion = await ValidarCuenta(cuenta);
            if (!resultadoValidacion.Equals("valida"))
                return BadRequest(new { message = resultadoValidacion });
            
            await _servicioCuenta.Actualizar(cuenta);
            return NoContent();
        }
        else
        {
            return CuentaNoEncontrada(id);
        }
    }

    [Authorize(Policy = "Superadmin")]
    [HttpDelete("borrar/{id}")]
    public async Task<IActionResult> Borrar(int id)
    {
        var cuentaABorrar = await _servicioCuenta.ObtenerPorId(id);
        if (cuentaABorrar is not null)
        {
            if (cuentaABorrar.Saldo > 0)
            {
                return BadRequest("La cuenta no debe tener saldo.");
            }

            await _servicioCuenta.Borrar(id);
            return Ok();
        }
        else
        {
            return CuentaNoEncontrada(id);
        }
    }

    private NotFoundObjectResult CuentaNoEncontrada(int id)
    {
        return NotFound(
            new { message = $"La cuenta con id. = {id} no existe." });
    }

    private async Task<string> ValidarCuenta(OTDCuentaEntrada cuenta)
    {
        string resultado = "valida";
        var tipoCuenta
            = await _servicioTipoCuenta.ObtenerPorId(cuenta.TipoCuenta);
        
        if (tipoCuenta is null)
            resultado = $"El tipo de cuenta {cuenta.TipoCuenta} no existe.";
        
        var idCliente = cuenta.IdCliente.GetValueOrDefault();
        var cliente = await _servicioCliente.ObtenerPorId(idCliente);

        if (cliente is null)
            resultado = $"El cliente {idCliente} no existe.";
        
        return resultado;
    }
}
