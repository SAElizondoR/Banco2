using System.Security.Claims;
using InterfazBanco.Datos.ModelosBanco;
using InterfazBanco.Datos.OTD;
using InterfazBanco.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterfazBanco.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OperacionesBancariasController : ControllerBase
{
    private readonly AccountService _servicioCuenta;
    private readonly ServicioTransacciones _servicioTransacciones;

    public OperacionesBancariasController(AccountService servicioCuenta,
        ServicioTransacciones servicioTransacciones)
    {
        this._servicioCuenta = servicioCuenta;
        this._servicioTransacciones = servicioTransacciones;
    }

    [HttpGet("consultar")]
    public async Task<IEnumerable<OTDCuentaSalida>?> ObtenerPorIdCliente(int id)
    {
        var idCliente = ObtenerIdCliente();

        return await _servicioCuenta.ObtenerOTDPorIdCliente(idCliente);
    }

    [HttpPost("retirar")]
    public async Task<IActionResult> Retirar(OTDTransaccionBancaria otd)
    {
        try
        {
            var idCliente = ObtenerIdCliente();
            var cuenta = await _servicioCuenta.ObtenerPorId(otd.IdCuenta);

            if (cuenta == null)
            {
                return NotFound("La cuenta no existe.");
            }

            if (cuenta.IdCliente != idCliente)
            {
                return Unauthorized(
                    "No tiene permiso para retirar de esta cuenta.");
            }

            if (otd.Importe > cuenta.Saldo)
            {
                return BadRequest("Saldo insuficiente.");
            }



            if (otd.CuentaExterna is not null)
            {
                var cuentaExterna
                    = await _servicioCuenta.ObtenerPorId(
                        (int)otd.CuentaExterna);
                if (cuentaExterna is null)
                {
                    return NotFound("La cuenta a depositar no existe.");
                }
                await _servicioCuenta.CambiarSaldo((int)otd.CuentaExterna,
                    otd.Importe);
            }

            await _servicioCuenta.CambiarSaldo(otd.IdCuenta, -otd.Importe);

            var transaccion = await _servicioTransacciones.Crear(otd);

            return Ok("Retiro exitoso.");
        }
        catch(Exception ex)
        {
            return StatusCode(500,$"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost("depositar")]
    public async Task<IActionResult> Depositar(OTDTransaccionBancaria otd)
    {
        var idCliente = ObtenerIdCliente();
        var cuenta = await _servicioCuenta.ObtenerPorId(otd.IdCuenta);

        if (cuenta == null)
        {
            return NotFound("La cuenta no existe.");
        }

        if (cuenta.IdCliente != idCliente)
        {
            return Unauthorized(
                "No tiene permiso para depositar en esta cuenta.");
        }

        await _servicioCuenta.CambiarSaldo(otd.IdCuenta, otd.Importe);

        var transaccion = await _servicioTransacciones.Crear(otd);

        return Ok("Depósito exitoso.");
    }

    private int ObtenerIdCliente()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null && int.TryParse(claim.Value, out var idCliente))
        {
            return idCliente;
        }

        throw new InvalidOperationException(
            "No se puede obtener el id. del cliente.");
    }
}