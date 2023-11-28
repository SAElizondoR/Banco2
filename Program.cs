using System.Text;
using InterfazBanco.Datos;
using InterfazBanco.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Contexto de la base de datos
builder.Services.AddSqlServer<BancoContext>(builder.Configuration
    .GetConnectionString("Banco"));

// Capa de servicios
builder.Services.AddScoped<ServicioCliente>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ServicioTipoCuenta>();
builder.Services.AddScoped<ServicioInicioSesion>();
builder.Services.AddScoped<ServicioTransacciones>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("aut", opciones => {
        opciones.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    .AddJwtBearer("cliente", opciones => {
        opciones.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(opciones => {
    opciones.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("aut", "cliente")
        .Build();
    opciones.AddPolicy("Superadmin",
        politica => politica.RequireClaim("TipoAdmin", "su"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
