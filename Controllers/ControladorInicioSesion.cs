using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InterfazBanco.Datos.ModelosBanco;
using InterfazBanco.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InterfazBanco.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InicioSesionController : ControllerBase
{
    private readonly ServicioInicioSesion servicioInicioSesion;
    private readonly IConfiguration config;

    public InicioSesionController(ServicioInicioSesion servicioInicioSesion,
        IConfiguration config)
    {
        this.servicioInicioSesion = servicioInicioSesion;
        this.config = config;
    }

    [HttpPost("autentificar")]
    public async Task<IActionResult> IniciarSesion(OTDAdmin otdAdmin)
    {
        var admin = await servicioInicioSesion.ObtenerAdmin(otdAdmin);

        if (admin is null)
            return BadRequest(new { message = "Credenciales inválidas." });
        
        // generar un identificador
        string identificador = GenerarIdentificador(admin);

        return Ok(new { token = identificador });
    }

    [HttpPost("cliente")]
    public async Task<IActionResult> IniciarSesionCliente(OTDAdmin otdCliente)
    {
        var cliente = await servicioInicioSesion.ObtenerCliente(otdCliente);

        if (cliente is null)
            return BadRequest(new { message = "Credenciales inválidas." });
        
        string identificador = GenerarIdentificador(cliente);

        return Ok(new { token = identificador });
    }

    private string GenerarIdentificador(Administrador admin)
    {
        var valores = new[]
        {
            new Claim(ClaimTypes.Name, admin.Nombre),
            new Claim(ClaimTypes.Email, admin.CorreoE),
            new Claim("TipoAdmin", admin.TipoAdmin)
        };

        return GenerarIdentificador(valores);
    }

    private string GenerarIdentificador(Cliente cliente)
    {
        var valores = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nombre),
            new Claim(ClaimTypes.Email, cliente.CorreoE!)
        };

        return GenerarIdentificador(valores);
    }

    private string GenerarIdentificador(Claim[] valores)
    {
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            config.GetSection("JWT:Key").Value!));
        
        var credenciales = new SigningCredentials(clave,
            SecurityAlgorithms.HmacSha512Signature);
        
        var identificadorSeguridad = new JwtSecurityToken(
            claims: valores,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credenciales);
        
        return new JwtSecurityTokenHandler().WriteToken(
            identificadorSeguridad);
    }
}