using System;
using System.Collections.Generic;

namespace GYM_ITM.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Horario> IdHorarios { get; set; } = new List<Horario>();
}
