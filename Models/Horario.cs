using System;
using System.Collections.Generic;

namespace GYM_ITM.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? IdActividad { get; set; }

    public int? IdEspacio { get; set; }

    public virtual Actividades? IdActividadNavigation { get; set; }

    public virtual Espacio? IdEspacioNavigation { get; set; }

    public virtual ICollection<Usuario> IdUsuarios { get; set; } = new List<Usuario>();
}
