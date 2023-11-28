using System;
using System.Collections.Generic;

namespace InterfazBanco.Datos.ModelosBanco;

public partial class Administrador
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NumeroTelefono { get; set; } = null!;

    public string CorreoE { get; set; } = null!;

    public string Contra { get; set; } = null!;

    public string TipoAdmin { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }
}
