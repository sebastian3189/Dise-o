using System;
using System.Collections.Generic;

namespace GYM_ITM.Models;

public partial class Espacio
{
    public int IdEspacio { get; set; }

    public string? NombreEspacio { get; set; }

    public int? LimiteDisponibilidad { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

}
