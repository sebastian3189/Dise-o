using System;
using System.Collections.Generic;

namespace GYM_ITM.Models;

public partial class Actividades
{
    public int IdActividad { get; set; }

    public string? NombreActividad { get; set; }

    public string? DescripcionActividad { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}
