using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GYM_ITM.Models;

public partial class Actividades
{
    public int IdActividad { get; set; }

    [Required(ErrorMessage = "El nombre de la actividad es obligatorio.")]
    public string? NombreActividad { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public string? DescripcionActividad { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}
